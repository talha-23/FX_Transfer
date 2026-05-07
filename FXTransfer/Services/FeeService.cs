// FXTransfer/Services/FeeService.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using FXTransfer.Data;
using FXTransfer.Models.Entities;

namespace FXTransfer.Services
{
    public class FeeService : IFeeService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private const string CACHE_KEY = "FeeSettings";
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public FeeService(ApplicationDbContext dbContext, IMemoryCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<FeeSettings> GetFeeSettingsAsync()
        {
            // Try to get from cache first
            if (_cache.TryGetValue(CACHE_KEY, out FeeSettings cachedSettings))
            {
                return cachedSettings;
            }

            await _semaphore.WaitAsync();
            try
            {
                // Double-check cache after acquiring lock
                if (_cache.TryGetValue(CACHE_KEY, out cachedSettings))
                {
                    return cachedSettings;
                }

                // Get from database
                var config = await _dbContext.FeeConfigurations
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (config == null)
                {
                    // Create default settings if none exist
                    config = new FeeConfiguration
                    {
                        FeePercentage = 2.0m,
                        MinimumFee = 1.0m,
                        MaximumFee = 50.0m,
                        PremiumDiscount = 50,
                        LastUpdated = DateTime.UtcNow
                    };

                    _dbContext.FeeConfigurations.Add(config);
                    await _dbContext.SaveChangesAsync();
                }

                var settings = new FeeSettings
                {
                    FeePercentage = config.FeePercentage,
                    MinimumFee = config.MinimumFee,
                    MaximumFee = config.MaximumFee,
                    PremiumDiscount = config.PremiumDiscount
                };

                // Cache for 5 minutes
                _cache.Set(CACHE_KEY, settings, TimeSpan.FromMinutes(5));

                return settings;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task RefreshFeeSettingsAsync()
        {
            _cache.Remove(CACHE_KEY);
            await GetFeeSettingsAsync(); // Reload fresh data
        }

        public async Task<decimal> CalculateFee(decimal amount, bool isPremiumUser) // Changed to async Task<decimal>
        {
            var settings = await GetFeeSettingsAsync();

            // Calculate base fee percentage
            decimal feePercentage = isPremiumUser
                ? settings.FeePercentage * (100 - settings.PremiumDiscount) / 100
                : settings.FeePercentage;

            decimal calculatedFee = amount * feePercentage / 100;

            // Apply min/max limits
            calculatedFee = Math.Max(calculatedFee, settings.MinimumFee);
            calculatedFee = Math.Min(calculatedFee, settings.MaximumFee);

            return Math.Round(calculatedFee, 2);
        }
    }
}
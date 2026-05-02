using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FXTransfer.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace FXTransfer.Services.Implementations;

public class ExchangeRateApiService : ICurrencyRateService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly ILogger<ExchangeRateApiService> _logger;
    private readonly IWebHostEnvironment _environment;
    private const string API_BASE_URL = "https://api.exchangerate-api.com/v4/latest/";
    private const int MAX_RETRY_ATTEMPTS = 2;
    private const int CACHE_DURATION_MINUTES = 30;

    public ExchangeRateApiService(
        HttpClient httpClient,
        IMemoryCache cache,
        ILogger<ExchangeRateApiService> logger,
        IWebHostEnvironment environment)
    {
        _httpClient = httpClient;
        _cache = cache;
        _logger = logger;
        _environment = environment;
    }

    public async Task<Dictionary<string, decimal>> GetRatesAsync(string baseCurrency = "USD")
    {
        try
        {
            var cacheKey = $"exchange_rates_{baseCurrency}";
            if (_cache.TryGetValue(cacheKey, out Dictionary<string, decimal>? cachedRates) && cachedRates != null)
            {
                return cachedRates;
            }

            var rates = await FetchRatesWithRetryAsync(baseCurrency);
            _cache.Set(cacheKey, rates, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
            return rates;
        }
        catch (Exception)
        {
            return await LoadRatesFromJsonFallbackAsync(baseCurrency);
        }
    }

    private async Task<Dictionary<string, decimal>> FetchRatesWithRetryAsync(string baseCurrency)
    {
        for (int attempt = 1; attempt <= MAX_RETRY_ATTEMPTS; attempt++)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{API_BASE_URL}{baseCurrency}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;

                    if (root.TryGetProperty("rates", out var ratesElement))
                    {
                        var rates = new Dictionary<string, decimal>();
                        foreach (var property in ratesElement.EnumerateObject())
                        {
                            rates[property.Name] = property.Value.GetDecimal();
                        }
                        return rates;
                    }
                }

                if (attempt < MAX_RETRY_ATTEMPTS)
                {
                    await Task.Delay(1000 * attempt);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Attempt {attempt} failed");
                if (attempt == MAX_RETRY_ATTEMPTS) throw;
            }
        }

        throw new Exception("Failed to fetch rates");
    }

    private async Task<Dictionary<string, decimal>> LoadRatesFromJsonFallbackAsync(string baseCurrency)
    {
        try
        {
            var fallbackPath = Path.Combine(_environment.WebRootPath, "data", "rates_fallback.json");

            if (!File.Exists(fallbackPath))
            {
                return GetDefaultRates();
            }

            var json = await File.ReadAllTextAsync(fallbackPath);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("rates", out var ratesElement))
            {
                var rates = new Dictionary<string, decimal>();
                foreach (var property in ratesElement.EnumerateObject())
                {
                    rates[property.Name] = property.Value.GetDecimal();
                }

                // Convert if base currency is not USD
                if (baseCurrency != "USD" && rates.ContainsKey(baseCurrency))
                {
                    var baseRate = rates[baseCurrency];
                    var convertedRates = new Dictionary<string, decimal>();
                    foreach (var rate in rates)
                    {
                        convertedRates[rate.Key] = rate.Value / baseRate;
                    }
                    return convertedRates;
                }

                return rates;
            }

            return GetDefaultRates();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fallback failed");
            return GetDefaultRates();
        }
    }

    private Dictionary<string, decimal> GetDefaultRates()
    {
        return new Dictionary<string, decimal>
        {
            { "USD", 1.0m },
            { "EUR", 0.92m },
            { "GBP", 0.79m },
            { "PKR", 278.50m },
            { "INR", 83.20m },
            { "AED", 3.67m },
            { "SAR", 3.75m },
            { "CAD", 1.36m },
            { "AUD", 1.52m }
        };
    }

    public async Task<(decimal ConvertedAmount, decimal Rate)> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount)
    {
        var rates = await GetRatesAsync(fromCurrency);
        var rate = rates.GetValueOrDefault(toCurrency, 1);
        return (Math.Round(amount * rate, 2), Math.Round(rate, 4));
    }

    public async Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency)
    {
        var rates = await GetRatesAsync(fromCurrency);
        return rates.GetValueOrDefault(toCurrency, 1);
    }
}
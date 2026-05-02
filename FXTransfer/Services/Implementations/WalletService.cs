using FXTransfer.Data;
using FXTransfer.Models.Entities;
using FXTransfer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FXTransfer.Services.Implementations;

public class WalletService : IWalletService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<WalletService> _logger;

    public WalletService(ApplicationDbContext context, ILogger<WalletService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Wallet>> GetUserWalletsAsync(string userId)
    {
        try
        {
            return await _context.Wallets
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to get wallets for user {userId}");
            return new List<Wallet>();
        }
    }

    public async Task<decimal> GetBalanceAsync(string userId, string currencyCode)
    {
        try
        {
            var wallet = await _context.Wallets
                .FirstOrDefaultAsync(w => w.UserId == userId && w.CurrencyCode == currencyCode.ToUpper());
            return wallet?.Balance ?? 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to get balance for user {userId}");
            return 0;
        }
    }

    public async Task AddFundsAsync(string userId, string currencyCode, decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive", nameof(amount));

        try
        {
            var wallet = await _context.Wallets
                .FirstOrDefaultAsync(w => w.UserId == userId && w.CurrencyCode == currencyCode.ToUpper());

            if (wallet == null)
            {
                wallet = await CreateWalletAsync(userId, currencyCode);
            }

            wallet.AddFunds(amount);
            await _context.SaveChangesAsync(); // IMPORTANT: This saves to database

            _logger.LogInformation($"Added {amount} {currencyCode} to wallet for user {userId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to add funds for user {userId}");
            throw;
        }
    }

    public async Task DeductBalanceAsync(string userId, string currencyCode, decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive", nameof(amount));

        try
        {
            var wallet = await _context.Wallets
                .FirstOrDefaultAsync(w => w.UserId == userId && w.CurrencyCode == currencyCode.ToUpper());

            if (wallet == null)
                throw new InvalidOperationException($"No wallet found for {currencyCode}");

            wallet.DeductFunds(amount);
            await _context.SaveChangesAsync(); // IMPORTANT: This saves to database

            _logger.LogInformation($"Deducted {amount} {currencyCode} from wallet for user {userId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to deduct funds for user {userId}");
            throw;
        }
    }

    public async Task<Wallet> CreateWalletAsync(string userId, string currencyCode)
    {
        try
        {
            var wallet = new Wallet
            {
                UserId = userId,
                CurrencyCode = currencyCode.ToUpper(),
                Balance = 0,
                CreatedAt = DateTime.UtcNow
            };

            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync(); // IMPORTANT: This saves to database

            _logger.LogInformation($"Created {currencyCode} wallet for user {userId}");
            return wallet;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to create wallet for user {userId}");
            throw;
        }
    }
}
using FXTransfer.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FXTransfer.Services.Interfaces;

/// <summary>
/// SRP: Wallet management interface
/// ISP: Focused on wallet operations
/// </summary>
public interface IWalletService
{
    /// <summary>
    /// Gets all wallets for a user
    /// </summary>
    Task<List<Wallet>> GetUserWalletsAsync(string userId);

    /// <summary>
    /// Gets balance for specific currency
    /// </summary>
    Task<decimal> GetBalanceAsync(string userId, string currencyCode);

    /// <summary>
    /// Adds funds to wallet
    /// </summary>
    Task AddFundsAsync(string userId, string currencyCode, decimal amount);

    /// <summary>
    /// Deducts funds from wallet
    /// </summary>
    Task DeductBalanceAsync(string userId, string currencyCode, decimal amount);

    /// <summary>
    /// Creates a new wallet for a currency
    /// </summary>
    Task<Wallet> CreateWalletAsync(string userId, string currencyCode);
}
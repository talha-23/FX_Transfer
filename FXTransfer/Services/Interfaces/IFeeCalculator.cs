using FXTransfer.Models.Entities;
using System.Threading.Tasks;

namespace FXTransfer.Services.Interfaces;

/// <summary>
/// SRP: Fee calculation interface
/// OCP: New fee strategies can be added without modification
/// ISP: Focused on single responsibility
/// DIP: Abstractions for dependency injection
/// </summary>
public interface IFeeCalculator
{
    /// <summary>
    /// Calculates transfer fee based on amount, currencies, and user type
    /// </summary>
    /// <param name="amount">Transfer amount in source currency</param>
    /// <param name="fromCurrency">Source currency code</param>
    /// <param name="toCurrency">Target currency code</param>
    /// <param name="user">Current user (null for anonymous)</param>
    /// <returns>Calculated fee amount</returns>
    Task<decimal> CalculateFeeAsync(decimal amount, string fromCurrency, string toCurrency, ApplicationUser? user);

    /// <summary>
    /// Gets the current fee percentage for a user
    /// </summary>
    Task<decimal> GetFeePercentageAsync(ApplicationUser? user);
}
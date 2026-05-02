using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FXTransfer.Services.Interfaces;

/// <summary>
/// SRP: Handles currency exchange rate operations
/// OCP: Open for extension with new rate providers
/// ISP: Focused interface for rate operations only
/// </summary>
public interface ICurrencyRateService
{
    /// <summary>
    /// Gets exchange rates for a base currency
    /// </summary>
    /// <param name="baseCurrency">Base currency code (e.g., USD)</param>
    /// <returns>Dictionary of currency codes to exchange rates</returns>
    /// <exception cref="ApiFailureException">Thrown when API fails and fallback also fails</exception>
    Task<Dictionary<string, decimal>> GetRatesAsync(string baseCurrency = "USD");

    /// <summary>
    /// Converts an amount from one currency to another
    /// </summary>
    /// <param name="fromCurrency">Source currency code</param>
    /// <param name="toCurrency">Target currency code</param>
    /// <param name="amount">Amount to convert</param>
    /// <returns>Converted amount and exchange rate</returns>
    Task<(decimal ConvertedAmount, decimal Rate)> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount);

    /// <summary>
    /// Gets the exchange rate between two currencies
    /// </summary>
    Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency);
}
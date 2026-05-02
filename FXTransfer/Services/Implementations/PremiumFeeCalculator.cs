using System.Threading.Tasks;
using FXTransfer.Models.Entities;
using FXTransfer.Services.Interfaces;

namespace FXTransfer.Services.Implementations;

/// <summary>
/// OCP: Extension of IFeeCalculator for premium users with 50% discount
/// LSP: Can replace IFeeCalculator without breaking functionality
/// </summary>
public class PremiumFeeCalculator : IFeeCalculator
{
    private readonly StandardFeeCalculator _standardCalculator;
    private const decimal PREMIUM_DISCOUNT = 0.5m; // 50% off

    public PremiumFeeCalculator()
    {
        _standardCalculator = new StandardFeeCalculator();
    }

    public async Task<decimal> CalculateFeeAsync(decimal amount, string fromCurrency, string toCurrency, ApplicationUser? user)
    {
        var standardFee = await _standardCalculator.CalculateFeeAsync(amount, fromCurrency, toCurrency, user);
        var discountedFee = standardFee * (1 - PREMIUM_DISCOUNT);

        return discountedFee;
    }

    public async Task<decimal> GetFeePercentageAsync(ApplicationUser? user)
    {
        var standardPercentage = await _standardCalculator.GetFeePercentageAsync(user);
        return standardPercentage * (1 - PREMIUM_DISCOUNT);
    }
}
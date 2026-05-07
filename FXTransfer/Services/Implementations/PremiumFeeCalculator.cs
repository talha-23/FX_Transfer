using System.Threading.Tasks;
using FXTransfer.Models.Entities;
using FXTransfer.Services.Interfaces;

namespace FXTransfer.Services.Implementations;

public class PremiumFeeCalculator : IFeeCalculator
{
    private readonly StandardFeeCalculator _standardCalculator;
    private const decimal PREMIUM_DISCOUNT = 0.5m; // 50% discount

    public PremiumFeeCalculator(StandardFeeCalculator standardCalculator)
    {
        _standardCalculator = standardCalculator;
    }

    public async Task<decimal> CalculateFeeAsync(decimal amount, string fromCurrency, string toCurrency, ApplicationUser? user)
    {
        var standardFee = await _standardCalculator.CalculateFeeAsync(amount, fromCurrency, toCurrency, user);
        return standardFee * (1 - PREMIUM_DISCOUNT);
    }

    public async Task<decimal> GetFeePercentageAsync(ApplicationUser? user)
    {
        var standardPercentage = await _standardCalculator.GetFeePercentageAsync(user);
        return standardPercentage * (1 - PREMIUM_DISCOUNT);
    }
}
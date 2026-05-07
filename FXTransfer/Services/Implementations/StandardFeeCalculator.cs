using System.Threading.Tasks;
using FXTransfer.Models.Entities;
using FXTransfer.Services.Interfaces;

namespace FXTransfer.Services.Implementations;

public class StandardFeeCalculator : IFeeCalculator
{
    private const decimal DEFAULT_FEE_PERCENTAGE = 2.0m;  // 2% for regular users
    private const decimal MINIMUM_FEE = 1.0m;
    private const decimal MAXIMUM_FEE = 50.0m;

    public async Task<decimal> CalculateFeeAsync(decimal amount, string fromCurrency, string toCurrency, ApplicationUser? user)
    {
        var feePercentage = await GetFeePercentageAsync(user);
        var calculatedFee = amount * (feePercentage / 100);

        if (calculatedFee < MINIMUM_FEE)
            calculatedFee = MINIMUM_FEE;
        else if (calculatedFee > MAXIMUM_FEE)
            calculatedFee = MAXIMUM_FEE;

        return calculatedFee;
    }

    public Task<decimal> GetFeePercentageAsync(ApplicationUser? user)
    {
        return Task.FromResult(DEFAULT_FEE_PERCENTAGE);  // Returns 2%
    }
}
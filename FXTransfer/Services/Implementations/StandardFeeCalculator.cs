using FXTransfer.Data;
using FXTransfer.Models.Entities;
using FXTransfer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FXTransfer.Services.Implementations;

public class StandardFeeCalculator : IFeeCalculator
{
    private readonly ApplicationDbContext _context;
    private const decimal MINIMUM_FEE = 1.0m;
    private const decimal MAXIMUM_FEE = 50.0m;

    public StandardFeeCalculator(ApplicationDbContext context)
    {
        _context = context;
    }

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

    public async Task<decimal> GetFeePercentageAsync(ApplicationUser? user)
    {
        var config = await _context.FeeConfigurations.FirstOrDefaultAsync();
        return config?.FeePercentage ?? 2.0m;
    }
}
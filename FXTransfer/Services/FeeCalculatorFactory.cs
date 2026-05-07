using FXTransfer.Models.Entities;
using FXTransfer.Services.Implementations;
using FXTransfer.Services.Interfaces;

namespace FXTransfer.Services;

public class FeeCalculatorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public FeeCalculatorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IFeeCalculator GetCalculator(ApplicationUser? user)
    {
        if (user?.IsPremium == true)
        {
            return _serviceProvider.GetRequiredService<PremiumFeeCalculator>();
        }
        return _serviceProvider.GetRequiredService<StandardFeeCalculator>();
    }
}
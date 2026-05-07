// FXTransfer/Services/IFeeService.cs
namespace FXTransfer.Services
{
    public interface IFeeService
    {
        Task<FeeSettings> GetFeeSettingsAsync();
        Task RefreshFeeSettingsAsync();
        Task<decimal> CalculateFee(decimal amount, bool isPremiumUser); // Changed to Task<decimal>
    }

    public class FeeSettings
    {
        public decimal FeePercentage { get; set; }
        public decimal MinimumFee { get; set; }
        public decimal MaximumFee { get; set; }
        public int PremiumDiscount { get; set; }
    }
}
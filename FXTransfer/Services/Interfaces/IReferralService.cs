using FXTransfer.Models.Entities;
using System.Threading.Tasks;

namespace FXTransfer.Services.Interfaces;

public interface IReferralService
{
    Task<string> GenerateReferralCodeAsync(string userId);
    Task<bool> ApplyReferralCodeAsync(string userId, string referralCode);
    Task<decimal> GetReferralBonusAsync(string userId);
    Task AddReferralBonusAsync(string referrerId, decimal amount);
}
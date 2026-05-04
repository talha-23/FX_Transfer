using FXTransfer.Data;
using FXTransfer.Models.Entities;
using FXTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FXTransfer.Services.Implementations;

public class ReferralService : IReferralService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ReferralService> _logger;
    private const decimal REFERRAL_BONUS = 5.00m;

    public ReferralService(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        ILogger<ReferralService> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<string> GenerateReferralCodeAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return string.Empty;

        if (string.IsNullOrEmpty(user.ReferralCode))
        {
            user.ReferralCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            await _userManager.UpdateAsync(user);
        }

        return user.ReferralCode;
    }

    public async Task<bool> ApplyReferralCodeAsync(string userId, string referralCode)
    {
        try
        {
            var referrer = await _userManager.Users
                .FirstOrDefaultAsync(u => u.ReferralCode == referralCode && u.Id != userId);

            if (referrer == null) return false;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !string.IsNullOrEmpty(user.ReferredBy)) return false;

            user.ReferredBy = referrer.Id;
            await _userManager.UpdateAsync(user);

            // Add bonus to referrer
            await AddReferralBonusAsync(referrer.Id, REFERRAL_BONUS);

            _logger.LogInformation($"Referral applied: {referrer.Email} referred {user.Email}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error applying referral code");
            return false;
        }
    }

    public async Task<decimal> GetReferralBonusAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user?.TotalBonusEarned ?? 0;
    }

    public async Task AddReferralBonusAsync(string referrerId, decimal amount)
    {
        var referrer = await _userManager.FindByIdAsync(referrerId);
        if (referrer != null)
        {
            referrer.TotalBonusEarned += amount;
            await _userManager.UpdateAsync(referrer);

            // Add to wallet
            var wallet = await _context.Wallets
                .FirstOrDefaultAsync(w => w.UserId == referrerId && w.CurrencyCode == "USD");

            if (wallet != null)
            {
                wallet.AddFunds(amount);
                await _context.SaveChangesAsync();
            }
        }
    }
}
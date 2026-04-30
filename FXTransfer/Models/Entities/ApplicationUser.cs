using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography.Xml;

namespace FXTransfer.Models.Entities;

/// <summary>
/// SRP: This class represents user entity with authentication properties
/// Encapsulation: All fields have controlled access via properties
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// User's full name (First and Last)
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Unique referral code for invite system
    /// </summary>
    public string? ReferralCode { get; set; }

    /// <summary>
    /// User ID who referred this user (null if no referral)
    /// </summary>
    public string? ReferredBy { get; set; }

    /// <summary>
    /// Whether the user account is suspended
    /// </summary>
    public bool IsSuspended { get; set; } = false;

    /// <summary>
    /// Registration timestamp (UTC)
    /// </summary>
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// User's country code for fee calculation (e.g., "US", "PK")
    /// </summary>
    public string? CountryCode { get; set; }

    /// <summary>
    /// Whether user has premium subscription
    /// </summary>
    public bool IsPremium { get; set; } = false;

    /// <summary>
    /// Premium subscription expiry date (null if not premium)
    /// </summary>
    public DateTime? PremiumExpiry { get; set; }

    /// <summary>
    /// Total referral bonus earned (in USD equivalent)
    /// </summary>
    public decimal TotalBonusEarned { get; set; } = 0;

    // Navigation properties
    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
    public virtual ICollection<Transfer> Transfers { get; set; } = new List<Transfer>();
}
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FXTransfer.Models.Entities;

/// <summary>
/// SRP: Application user entity extending IdentityUser
/// </summary>
public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public string? ReferralCode { get; set; }
    public string? ReferredBy { get; set; }
    public bool IsSuspended { get; set; } = false;
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public string? CountryCode { get; set; }
    public bool IsPremium { get; set; } = false;
    public DateTime? PremiumExpiry { get; set; }
    public decimal TotalBonusEarned { get; set; } = 0;

    // Add these properties to existing ApplicationUser class

    /// <summary>
    /// Wallet PIN for secure access (4-6 digits)
    /// </summary>
    public string? WalletPIN { get; set; }

    /// <summary>
    /// Whether Wallet PIN is enabled
    /// </summary>
    public bool IsWalletPINEnabled { get; set; } = false;

    /// <summary>
    /// Transaction PIN for sending money (4-6 digits)
    /// </summary>
    public string? TransactionPIN { get; set; }

    /// <summary>
    /// Whether Transaction PIN is enabled
    /// </summary>
    public bool IsTransactionPINEnabled { get; set; } = false;

    /// <summary>
    /// Failed PIN attempts count
    /// </summary>
    public int FailedPINAttempts { get; set; } = 0;

    /// <summary>
    /// PIN last changed date
    /// </summary>
    public DateTime? PINLastChanged { get; set; }

    // Navigation properties
    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
    public virtual ICollection<Transfer> Transfers { get; set; } = new List<Transfer>();
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FXTransfer.Models.Entities;

/// <summary>
/// SRP: Represents user's multi-currency wallet
/// Encapsulation: Balance changes only through methods
/// </summary>
public class Wallet
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [MaxLength(3)]
    public string CurrencyCode { get; set; } = string.Empty;

    private decimal _balance = 0;

    /// <summary>
    /// Current balance in this currency
    /// Encapsulation: Validation in property setter
    /// </summary>
    public decimal Balance
    {
        get => _balance;
        set
        {
            if (value < 0)
                throw new ArgumentException("Balance cannot be negative", nameof(value));
            _balance = Math.Round(value, 2);
        }
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? LastUpdatedAt { get; set; }

    // Navigation properties
    [ForeignKey("UserId")]
    public virtual ApplicationUser? User { get; set; }

    /// <summary>
    /// Adds funds to wallet with validation
    /// </summary>
    public void AddFunds(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive", nameof(amount));

        Balance += amount;
        LastUpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deducts funds from wallet with validation
    /// </summary>
    public void DeductFunds(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive", nameof(amount));

        if (Balance < amount)
            throw new InvalidOperationException($"Insufficient balance. Available: {Balance}, Required: {amount}");

        Balance -= amount;
        LastUpdatedAt = DateTime.UtcNow;
    }
}
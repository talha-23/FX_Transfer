using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FXTransfer.Models.Entities;

public class Wallet
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [MaxLength(3)]
    public string CurrencyCode { get; set; } = string.Empty;

    public decimal Balance { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? LastUpdatedAt { get; set; }

    [ForeignKey("UserId")]
    public virtual ApplicationUser? User { get; set; }

    public void AddFunds(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive", nameof(amount));

        Balance += amount;
        LastUpdatedAt = DateTime.UtcNow;
    }

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
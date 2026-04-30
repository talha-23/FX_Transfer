using FXTransfer.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FXTransfer.Models.Entities;

/// <summary>
/// SRP: Represents a currency transfer transaction
/// Encapsulation: All fields with validation
/// </summary>
public class Transfer
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [MaxLength(3)]
    public string FromCurrency { get; set; } = string.Empty;

    [Required]
    [MaxLength(3)]
    public string ToCurrency { get; set; } = string.Empty;

    private decimal _amount;

    /// <summary>
    /// Amount to transfer in source currency
    /// Encapsulation: Validation in property setter
    /// </summary>
    [Required]
    [Range(10, 50000)]
    public decimal Amount
    {
        get => _amount;
        set
        {
            if (value < 10)
                throw new ArgumentException("Minimum transfer amount is $10", nameof(value));
            if (value > 50000)
                throw new ArgumentException("Maximum transfer amount is $50,000", nameof(value));
            _amount = value;
        }
    }

    public decimal ConvertedAmount { get; set; }

    public decimal ExchangeRate { get; set; }

    public decimal Fee { get; set; }

    public decimal TotalAmount { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = TransferStatus.Pending.ToString();

    public string? ReceiptPath { get; set; }

    public string? QrCodePath { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? CompletedAt { get; set; }

    [EmailAddress]
    public string? RecipientEmail { get; set; }

    public string? RecipientName { get; set; }

    public bool RequiresApproval { get; set; } = false;

    public bool IsFlagged { get; set; } = false;

    public string? FlagReason { get; set; }

    public string? IpAddress { get; set; }

    public string? Geolocation { get; set; }

    // Navigation properties
    [ForeignKey("UserId")]
    public virtual ApplicationUser? User { get; set; }
}
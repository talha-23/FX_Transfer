using FXTransfer.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace FXTransfer.Models.DTOs;

/// <summary>
/// DTO for transfer request
/// </summary>
public class TransferRequest
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [MaxLength(3)]
    public string FromCurrency { get; set; } = string.Empty;

    [Required]
    [MaxLength(3)]
    public string ToCurrency { get; set; } = string.Empty;

    [Required]
    [Range(10, 50000)]
    public decimal Amount { get; set; }

    [EmailAddress]
    public string? RecipientEmail { get; set; }

    public string? RecipientName { get; set; }

    public string? IpAddress { get; set; }

    public string? Geolocation { get; set; }

    public ApplicationUser? User { get; set; }
}
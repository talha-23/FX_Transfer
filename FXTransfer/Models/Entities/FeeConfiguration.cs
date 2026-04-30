using System.ComponentModel.DataAnnotations;

namespace FXTransfer.Models.Entities;

/// <summary>
/// SRP: Manages fee configuration for transfers
/// OCP: New fee rules can be added without modifying this class
/// </summary>
public class FeeConfiguration
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Country code (null = global default)
    /// </summary>
    [MaxLength(2)]
    public string? CountryCode { get; set; }

    /// <summary>
    /// Fee percentage (e.g., 2.0 = 2%)
    /// </summary>
    [Range(0, 10)]
    public decimal FeePercentage { get; set; } = 2.0m;

    /// <summary>
    /// Minimum fee amount in USD
    /// </summary>
    [Range(0, 100)]
    public decimal MinimumFee { get; set; } = 1.0m;

    /// <summary>
    /// Maximum fee amount in USD
    /// </summary>
    [Range(0, 1000)]
    public decimal MaximumFee { get; set; } = 50.0m;

    public bool IsActive { get; set; } = true;

    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    [MaxLength(100)]
    public string? UpdatedBy { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FXTransfer.Models.Entities;

/// <summary>
/// SRP: Represents a rate alert set by a user
/// </summary>
public class RateAlert
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

    [Required]
    public decimal TargetRate { get; set; }

    public bool IsTriggered { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? TriggeredAt { get; set; }

    [MaxLength(20)]
    public string AlertType { get; set; } = "Above"; // Above, Below

    [ForeignKey("UserId")]
    public virtual ApplicationUser? User { get; set; }
}
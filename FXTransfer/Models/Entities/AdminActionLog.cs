using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FXTransfer.Models.Entities;

/// <summary>
/// SRP: Logs all admin actions for audit trail
/// </summary>
public class AdminActionLog
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string AdminUserId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Action { get; set; } = string.Empty;

    public string? TargetUserId { get; set; }

    public string? Details { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public string? IpAddress { get; set; }

    [ForeignKey("AdminUserId")]
    public virtual ApplicationUser? AdminUser { get; set; }

    [ForeignKey("TargetUserId")]
    public virtual ApplicationUser? TargetUser { get; set; }
}
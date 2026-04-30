using System.ComponentModel.DataAnnotations;

namespace FXTransfer.Models.DTOs;

/// <summary>
/// Data Transfer Object for registration requests
/// </summary>
public class RegisterDto
{
    [Required]
    [Display(Name = "Full Name")]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [MaxLength(2)]
    [Display(Name = "Country Code (US, PK, etc.)")]
    public string? CountryCode { get; set; }

    [Display(Name = "Referral Code (optional)")]
    public string? ReferralCode { get; set; }
}
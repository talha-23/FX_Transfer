using System.ComponentModel.DataAnnotations;

namespace FXTransfer.Models.DTOs;

/// <summary>
/// Data Transfer Object for login requests
/// </summary>
public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; } = false;
}
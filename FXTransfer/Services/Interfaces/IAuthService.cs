using FXTransfer.Models.DTOs;

namespace FXTransfer.Services.Interfaces;

public interface IAuthService
{
    Task<bool> LoginAsync(LoginDto loginDto);
    Task<(bool Success, string ErrorMessage)> RegisterAsync(RegisterDto registerDto);
    Task LogoutAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<string?> GetCurrentUserEmailAsync();
    Task<string?> GetCurrentUserFullNameAsync();
    Task<IList<string>> GetCurrentUserRolesAsync();
    Task<string?> GetCurrentUserIdAsync();  // ADD THIS LINE
}
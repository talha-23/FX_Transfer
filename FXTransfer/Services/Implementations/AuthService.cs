using System.Security.Claims;
using FXTransfer.Models.DTOs;
using FXTransfer.Models.Entities;
using FXTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Identity;


namespace FXTransfer.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly FxAuthenticationStateProvider _authProvider;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        FxAuthenticationStateProvider authProvider,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authProvider = authProvider;
        _logger = logger;
    }

    public async Task<bool> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                _logger.LogWarning($"User not found: {loginDto.Email}");
                return false;
            }

            if (user.IsSuspended)
            {
                _logger.LogWarning($"Suspended user: {loginDto.Email}");
                return false;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userInfo = new UserInfo
                {
                    Email = user.Email ?? string.Empty,
                    UserId = user.Id,
                    Role = roles.FirstOrDefault() ?? "Regular",
                    FullName = user.FullName
                };

                await _authProvider.MarkUserAsAuthenticated(userInfo);
                _logger.LogInformation($"User logged in: {loginDto.Email}");
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login error");
            return false;
        }
    }

    public async Task<(bool Success, string ErrorMessage)> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                CountryCode = registerDto.CountryCode,
                RegisteredAt = DateTime.UtcNow,
                ReferralCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper()
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Regular");

                var userInfo = new UserInfo
                {
                    Email = user.Email ?? string.Empty,
                    UserId = user.Id,
                    Role = "Regular",
                    FullName = user.FullName
                };

                await _authProvider.MarkUserAsAuthenticated(userInfo);
                return (true, string.Empty);
            }

            return (false, string.Join(", ", result.Errors.Select(e => e.Description)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Register error");
            return (false, "Registration failed");
        }
    }

    public async Task LogoutAsync()
    {
        await _authProvider.MarkUserAsLoggedOut();
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var authState = await _authProvider.GetAuthenticationStateAsync();
        return authState.User.Identity?.IsAuthenticated ?? false;
    }

    public async Task<string?> GetCurrentUserEmailAsync()
    {
        var authState = await _authProvider.GetAuthenticationStateAsync();
        return authState.User.FindFirst(ClaimTypes.Email)?.Value;
    }

    public async Task<string?> GetCurrentUserFullNameAsync()
    {
        var authState = await _authProvider.GetAuthenticationStateAsync();
        var email = authState.User.FindFirst(ClaimTypes.Email)?.Value;

        if (!string.IsNullOrEmpty(email))
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user?.FullName;
        }
        return null;
    }

    public async Task<IList<string>> GetCurrentUserRolesAsync()
    {
        var authState = await _authProvider.GetAuthenticationStateAsync();
        var email = authState.User.FindFirst(ClaimTypes.Email)?.Value;

        if (!string.IsNullOrEmpty(email))
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return await _userManager.GetRolesAsync(user);
            }
        }
        return new List<string>();
    }

    public async Task<string?> GetCurrentUserIdAsync()
    {
        var authState = await _authProvider.GetAuthenticationStateAsync();
        return authState.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
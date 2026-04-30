using FXTransfer.Models.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FXTransfer.Services;

/// <summary>
/// SRP: Provides authentication state for Blazor components
/// </summary>
public class IdentityAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<IdentityAuthenticationStateProvider> _logger;

    public IdentityAuthenticationStateProvider(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ILogger<IdentityAuthenticationStateProvider> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = _signInManager.Context.User;

        if (user?.Identity?.IsAuthenticated == true)
        {
            var appUser = await _userManager.GetUserAsync(user);
            if (appUser != null && !appUser.IsSuspended)
            {
                return new AuthenticationState(user);
            }
        }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public void NotifyUserAuthentication(string email)
    {
        var user = _signInManager.Context.User;
        var authState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }
}
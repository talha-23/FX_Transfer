using FXTransfer.Models.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FXTransfer.Services;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var user = _signInManager.Context.User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                var appUser = await _userManager.GetUserAsync(user);
                if (appUser != null && !appUser.IsSuspended)
                {
                    _currentUser = user;
                    return new AuthenticationState(user);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetAuthenticationStateAsync error: {ex.Message}");
        }

        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        return new AuthenticationState(_currentUser);
    }

    public void MarkUserAsAuthenticated(ClaimsPrincipal user)
    {
        _currentUser = user;
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void MarkUserAsLoggedOut()
    {
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }
}
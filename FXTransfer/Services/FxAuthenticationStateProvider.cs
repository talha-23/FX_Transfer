using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace FXTransfer.Services;

public class FxAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _jsRuntime;
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public FxAuthenticationStateProvider(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "fx_user");

            if (!string.IsNullOrEmpty(userJson))
            {
                var userInfo = JsonSerializer.Deserialize<UserInfo>(userJson);
                if (userInfo != null && !string.IsNullOrEmpty(userInfo.Email))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userInfo.Email),
                        new Claim(ClaimTypes.Email, userInfo.Email),
                        new Claim(ClaimTypes.NameIdentifier, userInfo.UserId),
                        new Claim(ClaimTypes.Role, userInfo.Role)
                    };

                    var identity = new ClaimsIdentity(claims, "FXAuth");
                    var user = new ClaimsPrincipal(identity);
                    return new AuthenticationState(user);
                }
            }
        }
        catch (Exception)
        {
            // Handle error
        }

        return new AuthenticationState(_anonymous);
    }

    public async Task MarkUserAsAuthenticated(UserInfo userInfo)
    {
        var userJson = JsonSerializer.Serialize(userInfo);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "fx_user", userJson);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userInfo.Email),
            new Claim(ClaimTypes.Email, userInfo.Email),
            new Claim(ClaimTypes.NameIdentifier, userInfo.UserId),
            new Claim(ClaimTypes.Role, userInfo.Role)
        };

        var identity = new ClaimsIdentity(claims, "FXAuth");
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "fx_user");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }
}

public class UserInfo
{
    public string Email { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}
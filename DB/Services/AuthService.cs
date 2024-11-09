using System.Security.Claims;

using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.Authorization;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    public CustomAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }


    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = await _localStorage.GetItemAsync<DB.Models.AuthModel>("auth_token");

        ClaimsIdentity identity;
        if (user != null)
        {
            identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Rola.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            }, "apiauth_type");
        }
        else
        {
            identity = new ClaimsIdentity();
        }

        var claim = new ClaimsPrincipal(identity);
        return await Task.FromResult(new AuthenticationState(claim));
    }


    public async Task MarkUserAsLoggedOut()
    {
        await _localStorage.RemoveItemAsync("auth_token");
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }
}
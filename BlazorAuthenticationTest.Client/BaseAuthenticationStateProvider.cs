using System.Net.Http.Json;
using System.Security.Claims;

using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorAuthenticationTest.Client;

public abstract class BaseAuthenticationStateProvider : AuthenticationStateProvider
{
    protected readonly HttpClient _httpClient;

    protected BaseAuthenticationStateProvider(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(nameof(BaseAuthenticationStateProvider)); 
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = await _httpClient.GetFromJsonAsync<User>("api/account/getuser");
        var identity = CreateClaimsIdentity(user);
        var userPrincipal = new ClaimsPrincipal(identity);
        return new AuthenticationState(userPrincipal);
    }

    // Common implementation shared across both server and client

    protected ClaimsIdentity CreateClaimsIdentity(User user)
    {
        if (user != null && user.IsAuthenticated)
        {
            return new ClaimsIdentity(new[]
                                          {
                                              new Claim(ClaimTypes.Name, user.Email),
                                              // Additional claims can be added here
                                          }, GetAuthenticationType());
        }
        return new ClaimsIdentity();
    }

    protected abstract string GetAuthenticationType();

    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
using System.Net.Http.Json;
using System.Security.Claims;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorAuthenticationTest.Client;

public class CustomAuthStateProviderClient : BaseAuthenticationStateProvider
{
    public CustomAuthStateProviderClient(IHttpClientFactory httpClientFactory)
        : base(httpClientFactory)
    {
    }

    protected override string GetAuthenticationType()
    {
        //return "jwt"; // Client-side authentication type
        return CookieAuthenticationDefaults.AuthenticationScheme;
    }
}
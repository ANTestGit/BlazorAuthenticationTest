using BlazorAuthenticationTest.Client;

using Microsoft.AspNetCore.Authentication.Cookies;

namespace BlazorAuthenticationTest;

public class CustomAuthStateProviderServer : BaseAuthenticationStateProvider
{
    public CustomAuthStateProviderServer(IHttpClientFactory httpClientFactory)
        : base(httpClientFactory)
    {
    }

    protected override string GetAuthenticationType()
    {
        //return "cookie"; // Server-side authentication type
        return CookieAuthenticationDefaults.AuthenticationScheme;
    }
}

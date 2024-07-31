using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorAuthenticationTest.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.Services.AddHttpClient(nameof(BaseAuthenticationStateProvider), client =>
                {
                    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); // Use the base address from host environment
                });
            
            builder.Services.AddAuthorizationCore();
            // Register the custom authentication state provider
            //builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProviderClient>();
            await builder.Build().RunAsync();
        }
    }
}

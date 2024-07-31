using BlazorAuthenticationTest.Client;
using BlazorAuthenticationTest.Components;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorAuthenticationTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            // Read base address from configuration
            var baseAddress = builder.Configuration["ApiSettings:BaseAddress"];
            builder.Services.AddHttpClient<BaseAuthenticationStateProvider>( client => { client.BaseAddress = new Uri(baseAddress); });

            //builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProviderServer>();
            builder.Services.AddControllers();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(
                    options =>
                        {
                            options.LoginPath = "/login";
                            options.AccessDeniedPath = "/access-denied";
                        });

            builder.Services.AddAuthorizationCore();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}

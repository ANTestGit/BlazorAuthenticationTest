using System.Net.Http;
using System.Security.Claims;

using BlazorAuthenticationTest.Client;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAuthenticationTest.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        public const string FixedEmail = "user@example.com";

        public const string FixedPassword = "password";

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password)
        {
            if (email == FixedEmail && password == FixedPassword)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, email) };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
                var cookieOptions = new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(1), Secure = true };
                Response.Cookies.Append("APITest", "Test value", cookieOptions);

                //server-side redirect (Redirect method) inherently causes a full page load.
                return Redirect("/");
            }

            return Unauthorized();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult GetUser()
        {
            var user = new User
                           {
                               Email = User.Identity?.Name,
                               IsAuthenticated = User.Identity?.IsAuthenticated ?? false,
                               Roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value)
                           };

            return Ok(user);
        }
    }
}
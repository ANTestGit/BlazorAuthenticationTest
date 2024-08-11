# BlazorAuthenticationTest
I am working on a .NET 8.0 Blazor project utilizing the "auto" render mode, 
with cookie-based authentication. 
For authentication, we use cookies to manage user sessions. 
The authentication setup involves configuring cookie settings and implementing a custom AuthenticationStateProvider to manage the authentication state across the application. The project also includes server-side logic for handling login and logout actions

The CustomAuthenticationStateProvider manages the user's authentication state, while the AccountController handles login and logout operations.

The issue is that when I use HttpContext.SignInAsync in the controller, the cookie isn't set, and the authentication state isn't updated. If I move the login logic to the login page, everything works fine server-side, but I can't access the authenticated user from the client side via API calls.

# Response 1
Your Account/Login page is rendered on the server and calls an API on the server (using HttpClient) that's going to set a cookie in the response received by HttpClient, not in your browser.

For the cookie to be set in the browser you need to make the API call from code running in the browser (through WebAssembly) not code running on the server either via SSR or Interactive Server.

# Next try
So I try to use login in server-side page only
```csharp
private async Task HandleLogin()
    {
        if (loginModel.Email == AccountController.FixedEmail && loginModel.Password == AccountController.FixedPassword)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, loginModel.Email) };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
            

            Navigation.NavigateTo("/", true);
        }
        else
        {
            errorMessage = "Invalid login attempt.";
        }
    }
```
As I know:
A full page reload after setting the cookie is essential to ensure that all components and the Blazor infrastructure are aware of the new authentication state. This reload causes the browser to include the authentication cookie in the request, which the server uses to authenticate the user and return the appropriate UI components.

## Problem 2

When using a client-side component with the render mode `InteractiveAuto` on the server side:

```html
<UserInfo @rendermode="InteractiveAuto"/>
```
And injecting AuthenticationStateProvider within the component:

```csharp
@inject AuthenticationStateProvider AuthStateProvider
```
You might encounter a peculiar issue. Upon logging in or logging out, the following error appears:

```txt
Error: One or more errors occurred. (Cannot provide a value for property 'AuthStateProvider' on type 'BlazorAuthenticationTest.Client.Components.UserInfo'. There is no registered service of type 'Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider'.)
    at Jn (marshal-to-js.ts:349:18)
    at Tl (marshal-to-js.ts:306:28)
    at 00b21cf6:0x1fad7
    at 00b21cf6:0x1bf9f
    at 00b21cf6:0xf16c
    at 00b21cf6:0x1e7f1
    at 00b21cf6:0x1efe7
    at 00b21cf6:0xcfbc
    at 00b21cf6:0x44213
    at e.<computed> (cwraps.ts:338:24)
```

Interestingly, if you clear all cookies, the error disappears. In some cases, clearing the solution and rebuilding the project may also resolve the issue.
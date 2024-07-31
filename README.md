# BlazorAuthenticationTest
I am working on a .NET 8.0 Blazor project utilizing the "auto" render mode, 
with cookie-based authentication. 
For authentication, we use cookies to manage user sessions. 
The authentication setup involves configuring cookie settings and implementing a custom AuthenticationStateProvider to manage the authentication state across the application. The project also includes server-side logic for handling login and logout actions

The CustomAuthenticationStateProvider manages the user's authentication state, while the AccountController handles login and logout operations.

The issue is that when I use HttpContext.SignInAsync in the controller, the cookie isn't set, and the authentication state isn't updated. If I move the login logic to the login page, everything works fine server-side, but I can't access the authenticated user from the client side via API calls.
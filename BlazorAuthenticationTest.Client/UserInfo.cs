using System.Security.Claims;

namespace ActionManager.Client
{
    // Add properties to this class and update the server and client AuthenticationStateProviders
    // to expose more information about the authenticated user to the client.
    public class UserInfo
    {
        public required string UserId { get; set; }
        public required string Email { get; set; }
        
        public required string Name { get; set; }

        //public IEnumerable<Claim>? Roles { get; set; }
        public string Roles { get; set; }

    }
}

using Microsoft.AspNetCore.Authorization;

namespace Api.Configuration
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        public JwtAuthorizeAttribute(params string[] roles)
        {
            AuthenticationSchemes = "Bearer";
            Roles = roles.Length > 0 ? string.Join(",", roles) : Roles;
        }
    }
}

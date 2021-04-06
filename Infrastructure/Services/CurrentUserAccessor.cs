using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SsnDbContext _context;

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor, SsnDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        public ApplicationUser GetCurrentUser()
        {
            var userId = GetCurrentUserId();
            return _context.Users.Find(userId);
        }

        public ValueTask<ApplicationUser> GetCurrentUserAsync(CancellationToken cancellationToken = new())
        {
            var userId = GetCurrentUserId();
            return _context.Users.FindAsync(new object[] { userId }, cancellationToken);
        }

        public string GetIpAddress()
        {
            string ipAddress;
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                ipAddress = httpContext.Request.Headers["X-Forwarded-For"];
            else 
                ipAddress = httpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();

            return ipAddress;
        }
    }
}

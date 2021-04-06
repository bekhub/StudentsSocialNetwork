using System.Threading;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface ICurrentUserAccessor
    {
        string GetCurrentUserId();

        ApplicationUser GetCurrentUser();

        ValueTask<ApplicationUser> GetCurrentUserAsync(CancellationToken cancellationToken = new());

        string GetIpAddress();
    }
}

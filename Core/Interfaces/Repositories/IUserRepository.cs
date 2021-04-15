using System.Threading;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<(ApplicationUser, RefreshToken)> GetByActiveRefreshTokenAsync(string token, CancellationToken cancellationToken = default);

        Task<ApplicationUser> GetByUsernameAsync(string username);

        Task<ApplicationUser> GetByEmailAsync(string email);
    }
}

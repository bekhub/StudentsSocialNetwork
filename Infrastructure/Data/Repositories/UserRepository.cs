using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class UserRepository : EfRepository<ApplicationUser>, IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        
        public UserRepository(SsnDbContext ssnDbContext, UserManager<ApplicationUser> userManager) : base(ssnDbContext)
        {
            _userManager = userManager;
        }

        public async Task<(ApplicationUser, RefreshToken)> GetByActiveRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            var user = await SsnDbContext.Users
                .SingleOrDefaultAsync(x => 
                    x.RefreshTokens.Any(z => z.IsActive && z.Token == token), cancellationToken);
            var refreshToken = user?.RefreshTokens.Single(x => x.Token == token);
            return (user, refreshToken);
        }

        public Task<ApplicationUser> GetByUsernameAsync(string username)
        {
            return _userManager.FindByNameAsync(username);
        }

        public Task<ApplicationUser> GetByEmailAsync(string email)
        {
            return _userManager.FindByEmailAsync(email);
        }
    }
}

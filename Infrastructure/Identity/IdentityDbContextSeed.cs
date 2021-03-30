using System;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class IdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            var defaultUserName = "demouser@ssn.com";
            if (await userManager.FindByNameAsync(defaultUserName) == null)
            {
                var defaultUser = new ApplicationUser
                {
                    UserName = defaultUserName,
                    Email = defaultUserName,
                    SignUpDate = DateTime.UtcNow,
                };
                await userManager.CreateAsync(defaultUser, AuthorizationConstants.DEFAULT_PASSWORD);
                await userManager.AddToRoleAsync(defaultUser, RoleConstants.USER);
                await userManager.AddToRoleAsync(defaultUser, RoleConstants.STUDENT);
            }

            var adminUserName = "admin@ssn.com";
            if (await userManager.FindByNameAsync(adminUserName) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminUserName,
                    Email = adminUserName,
                    SignUpDate = DateTime.UtcNow,
                };
                await userManager.CreateAsync(adminUser, AuthorizationConstants.DEFAULT_PASSWORD);
                adminUser = await userManager.FindByNameAsync(adminUserName);
                await userManager.AddToRoleAsync(adminUser, RoleConstants.ADMINISTRATOR);
            }
        }
    }
}

using System;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class IdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, SsnDbContext context)
        {
            if (!await roleManager.RoleExistsAsync(RoleConstants.ADMINISTRATOR))
                await roleManager.CreateAsync(new IdentityRole(RoleConstants.ADMINISTRATOR));
            if (!await roleManager.RoleExistsAsync(RoleConstants.USER))
                await roleManager.CreateAsync(new IdentityRole(RoleConstants.USER));
            if (!await roleManager.RoleExistsAsync(RoleConstants.STUDENT))
                await roleManager.CreateAsync(new IdentityRole(RoleConstants.STUDENT));
            
            if (await userManager.FindByNameAsync(AuthorizationConstants.DEFAULT_USERNAME) == null)
            {
                var defaultUser = new ApplicationUser
                {
                    UserName = AuthorizationConstants.DEFAULT_USERNAME,
                    Email = AuthorizationConstants.DEFAULT_USERNAME,
                    SignUpDate = DateTime.UtcNow,
                };
                await userManager.CreateAsync(defaultUser, AuthorizationConstants.DEFAULT_PASSWORD);
                await userManager.AddToRoleAsync(defaultUser, RoleConstants.USER);
            }

            if (await userManager.FindByNameAsync(AuthorizationConstants.STUDENT_USERNAME) == null)
            {
                var studentUser = new ApplicationUser
                {
                    UserName = AuthorizationConstants.STUDENT_USERNAME,
                    Email = AuthorizationConstants.STUDENT_USERNAME,
                    SignUpDate = DateTime.UtcNow,
                };
                var institute = await context.Institutes.FirstOrDefaultAsync() ?? new Institute
                {
                    Name = "TestInstitute",
                };
                var department = await context.Departments.FirstOrDefaultAsync() ?? new Department
                {
                    Name = "TestDepartment",
                    Institute = institute,
                };
                var student = new Student
                {
                    StudentNumber = AuthorizationConstants.STUDENT_NUMBER,
                    StudentEmail = AuthorizationConstants.STUDENT_EMAIL,
                    StudentPassword = AuthorizationConstants.DEFAULT_PASSWORD,
                    Department = department,
                };
                studentUser.Students.Add(student);
                await userManager.CreateAsync(studentUser, AuthorizationConstants.DEFAULT_PASSWORD);
                await userManager.AddToRoleAsync(studentUser, RoleConstants.USER);
                await userManager.AddToRoleAsync(studentUser, RoleConstants.STUDENT);
            }

            if (await userManager.FindByNameAsync(AuthorizationConstants.ADMIN_USERNAME) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = AuthorizationConstants.ADMIN_USERNAME,
                    Email = AuthorizationConstants.ADMIN_USERNAME,
                    SignUpDate = DateTime.UtcNow,
                };
                await userManager.CreateAsync(adminUser, AuthorizationConstants.DEFAULT_PASSWORD);
                await userManager.AddToRoleAsync(adminUser, RoleConstants.ADMINISTRATOR);
            }
        }
    }
}

using Ardalis.EFCore.Extensions;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SsnDbContext : IdentityDbContext<ApplicationUser>
    {
        public SsnDbContext(DbContextOptions<SsnDbContext> options) : base(options) { }
        
        public SsnDbContext(DbContextOptions<SsnDbContext> options, IMediator mediator)
            : base(options)
        { }

        public DbSet<Student> Students { get; set; }
        
        public DbSet<Department> Departments { get; set; }

        public DbSet<Course> Courses { get; set; }
        
        public DbSet<CourseTime> CourseTimes { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }
        
        public DbSet<Assessment> Assessments { get; set; }
        
        public DbSet<Institute> Institutes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.ApplyAllConfigurationsFromCurrentAssembly();

            builder.Entity<ApplicationUser>(entity => entity.ToTable("Users"));
            builder.Entity<IdentityRole>(entity => entity.ToTable("Roles"));
            builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("UserRoles"));
            builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable("UserClaims"));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("UserLogins"));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable("RoleClaims"));
            builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable("UserTokens"));
        }
    }
}

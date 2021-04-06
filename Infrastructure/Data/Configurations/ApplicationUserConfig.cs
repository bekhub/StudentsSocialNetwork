using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasMany(x => x.Students)
                .WithOne(x => x.User);

            builder.OwnsMany(x => x.RefreshTokens, 
                o => o.HasKey(x => x.Id));
            
            builder.HasIndex(x => x.UserName)
                .IsUnique();
            builder.HasIndex(x => x.Email)
                .IsUnique();
        }
    }
}

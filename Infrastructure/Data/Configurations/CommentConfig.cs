using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(x => x.Body)
                .IsRequired();
            builder.Property(x => x.UserId)
                .IsRequired();
            builder.HasMany(x => x.Replies)
                .WithOne(x => x.Target)
                .HasForeignKey(x => x.TargetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

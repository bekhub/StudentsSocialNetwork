using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class PostPictureConfig : IEntityTypeConfiguration<PostPicture>
    {
        public void Configure(EntityTypeBuilder<PostPicture> builder)
        {
            builder.Property(x => x.Url)
                .IsRequired();
        }
    }
}

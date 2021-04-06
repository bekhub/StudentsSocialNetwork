using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Property(x => x.StudentNumber)
                .IsRequired();
            builder.Property(x => x.StudentEmail)
                .IsRequired();
            builder.Property(x => x.StudentPassword)
                .IsRequired();
            builder.Property(x => x.UserId)
                .IsRequired();

            builder.HasIndex(x => x.StudentNumber)
                .IsUnique();
        }
    }
}

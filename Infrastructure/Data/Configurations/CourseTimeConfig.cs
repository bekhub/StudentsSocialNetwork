using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class CourseTimeConfig : IEntityTypeConfiguration<CourseTime>
    {
        public void Configure(EntityTypeBuilder<CourseTime> builder)
        {
            builder.Property(x => x.Weekday)
                .HasConversion<int>();
        }
    }
}

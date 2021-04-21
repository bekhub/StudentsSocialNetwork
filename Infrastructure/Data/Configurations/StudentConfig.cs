using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasMany(x => x.Courses)
                .WithMany(x => x.Students)
                .UsingEntity<StudentCourse>(
                    x => x
                        .HasOne(sc => sc.Course)
                        .WithMany(c => c.StudentCourses)
                        .HasForeignKey(sc => sc.CourseId),
                    x => x
                        .HasOne(sc => sc.Student)
                        .WithMany(s => s.StudentCourses)
                        .HasForeignKey(sc => sc.StudentId),
                    x => x
                        .HasKey(sc => sc.Id)
                );
            
            builder.Property(x => x.StudentNumber)
                .IsRequired();
            builder.Property(x => x.StudentEmail)
                .IsRequired();
            builder.Property(x => x.StudentPassword)
                .IsRequired();

            builder.HasIndex(x => x.StudentNumber)
                .IsUnique();
        }
    }
}

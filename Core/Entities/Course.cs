using System.Collections.Generic;
using Core.Interfaces;

namespace Core.Entities
{
    public class Course : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; }
        
        public string Code { get; set; }

        public int Theory { get; set; }
        
        public int Practice { get; set; }

        public int Total => Theory + Practice;
        
        public int Credits { get; set; }
        
        public int? Grade { get; set; }

        public int? DepartmentId { get; set; }
        public Department Department { get; set; }

        public ICollection<CourseTime> CourseTimes { get; set; }

        public ICollection<Student> Students { get; set; }
        public List<StudentCourse> StudentCourses { get; set; }
    }
}

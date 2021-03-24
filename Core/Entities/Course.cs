using System.Collections.Generic;

namespace Core.Entities
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }
        
        public string Code { get; set; }
        
        public int Semester { get; set; }
        
        public int Theory { get; set; }
        
        public int Practice { get; set; }
        
        public int Credits { get; set; }
        
        public int Year { get; set; }

        public int DepartmentId { get; set; }
        
        public Department Department { get; set; }

        public int CourseTypeId { get; set; }

        public CourseType CourseType { get; set; }

        public IEnumerable<CourseTime> CourseTimes { get; set; }
    }
}

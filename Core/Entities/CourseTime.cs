using System;

namespace Core.Entities
{
    public class CourseTime : BaseEntity
    {
        public int Weekday { get; set; }

        public TimeSpan Time { get; set; }

        public DateTime AcademicYear { get; set; }

        public int CourseId { get; set; }
        
        public Course Course { get; set; }
    }
}

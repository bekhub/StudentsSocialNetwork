using System;
using Core.Interfaces;

namespace Core.Entities
{
    public class CourseTime : BaseEntity, IAggregateRoot
    {
        public int Weekday { get; set; }

        public TimeSpan Time { get; set; }

        public int AcademicYearFrom { get; set; }
        
        public int AcademicYearTo { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}

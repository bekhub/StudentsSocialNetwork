using System;
using Core.Enums;
using Core.Interfaces;

namespace Core.Entities
{
    public class CourseTime : BaseEntity, IAggregateRoot
    {
        public Weekday Weekday { get; set; }

        public TimeSpan TimeFrom { get; set; }
        
        public TimeSpan TimeTo { get; set; }

        public string Classroom { get; set; }

        public int StudentCourseId { get; set; }
        public StudentCourse StudentCourse { get; set; }
    }
}

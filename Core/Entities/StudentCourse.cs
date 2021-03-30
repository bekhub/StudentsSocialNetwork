using System.Collections.Generic;
using Core.Interfaces;

namespace Core.Entities
{
    public class StudentCourse : BaseEntity, IAggregateRoot
    {
        public int TheoryAbsent { get; set; }

        public int PracticeAbsent { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public ICollection<Assessment> Assessments { get; set; }
    }
}

using System;
using System.Collections.Generic;
using Core.Interfaces;

namespace Core.Entities
{
    public class StudentCourse : BaseEntity, IAggregateRoot
    {
        public int TheoryAbsent { get; set; }

        public int PracticeAbsent { get; set; }

        public bool IsActive => AcademicYear == CurrentAcademicYear && Semester == CurrentSemester;

        public int AcademicYear { get; set; }

        public Semester Semester { get; set; }

        public string AverageAssessment { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public ICollection<Assessment> Assessments { get; set; }
        
        public static Semester CurrentSemester => DateTime.UtcNow.Month is > 8 or < 2 ? Semester.First : Semester.Second;

        public static int CurrentAcademicYear => DateTime.UtcNow.Year;
    }

    public enum Semester
    {
        First = 1,
        Second = 2,
    }
}

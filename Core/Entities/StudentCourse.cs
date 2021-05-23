using System;
using System.Collections.Generic;
using Core.Enums;
using Core.Interfaces;

namespace Core.Entities
{
    public class StudentCourse : BaseEntity, IAggregateRoot
    {
        public int TheoryAbsent { get; set; }

        public double TheoryAbsentPercentage => CalculateAbsentPercentage(TheoryAbsent, Course.Theory);

        public int PracticeAbsent { get; set; }
        
        public double PracticeAbsentPercentage => CalculateAbsentPercentage(PracticeAbsent, Course.Practice);

        public bool IsActive => AcademicYear == CurrentAcademicYear && Semester == CurrentSemester;

        public int AcademicYear { get; set; }

        public Semester Semester { get; set; }

        public string AverageAssessment { get; set; }

        public string Teacher { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public ICollection<Assessment> Assessments { get; set; }
        
        public static Semester CurrentSemester => DateTime.UtcNow.Month is > 8 or < 2 ? Semester.First : Semester.Second;

        public static int CurrentAcademicYear => DateTime.UtcNow.Year;

        private static float CalculateAbsentPercentage(int absent, int coursesPerWeek)
        {
            if (coursesPerWeek == 0) return 0;
            return (float) absent * 100 / (16 * coursesPerWeek);
        }
    }
}

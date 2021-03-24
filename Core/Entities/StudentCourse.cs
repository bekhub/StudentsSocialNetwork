using Core.Interfaces;

namespace Core.Entities
{
    public class StudentCourse : BaseEntity, IAggregateRoot
    {
        public int TheoryAbsent { get; set; }

        public int PracticeAbsent { get; set; }

        public int FinalExam { get; set; }

        public int MidtermExam { get; set; }
        
        public int? MidtermExam2 { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}

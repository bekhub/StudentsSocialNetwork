using System.Collections.Generic;

namespace Core.ObisApiModels
{
    public class StudentSemesterNotes
    {
        public class Response
        {
            public List<Lesson> Lessons { get; set; }
        }
        
        public class Lesson
        {
            public string Name { get; set; }

            public List<Exam> Exams { get; set; }
        }

        public class Exam
        {
            public string Name { get; set; }
        
            public string Mark { get; set; }
        
            public string Avg { get; set; }
        }
    }
}

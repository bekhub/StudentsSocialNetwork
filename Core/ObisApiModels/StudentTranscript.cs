using System.Collections.Generic;

namespace Core.ObisApiModels
{
    public class StudentTranscript
    {
        public class Response
        {
            public Undergraduate Undergraduate { get; set; }
        }
        
        public class Undergraduate
        {
            public General General { get; set; }
            
            public List<Semester> Semesters { get; set; }
        }

        public class Semester
        {
            public string Name { get; set; }

            public string Gpa { get; set; }
            
            public string TotalCredit { get; set; }
            
            public string TotalAverage { get; set; }

            public List<Lesson> Lessons { get; set; }
        }

        public class Lesson
        {
            public string Code { get; set; }
            
            public string Name { get; set; }
            
            public string Mark { get; set; }
            
            public string Credit { get; set; }
            
            public string Supplement { get; set; }
        }
        
        public class General
        {
            public string Gpa { get; set; }
            
            public string TotalCredit { get; set; }
            
            public string TotalAverage { get; set; }
        }
    }
}

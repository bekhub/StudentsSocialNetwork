using System;
using System.Collections.Generic;

namespace Api.Endpoints.StudentAccount
{
    public class Response
    {
        public class PersonalInformation
        {
            public string StudentNumber { get; set; }

            public string Fullname { get; set; }

            public string Email { get; set; }

            public DateTime? Birthdate { get; set; }

            public string Department { get; set; }

            public string Faculty { get; set; }
        }
        
        public class LessonsAndMarks
        {
            public string Code { get; set; }

            public string Name { get; set; }

            public int TheoryAbsent { get; set; }
            
            public double TheoryAbsentPercentage { get; set; }
            
            public int PracticeAbsent { get; set; }
            
            public double PracticeAbsentPercentage { get; set; }

            public string AverageMark { get; set; }
            
            public List<Mark> Marks { get; set; }
        }
        
        public class Mark
        {
            public string Name { get; set; }
            
            public int Point { get; set; }
        }
    }
}

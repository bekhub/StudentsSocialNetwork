using System.Collections.Generic;

namespace Api.Endpoints.StudentAccount
{
    public class Response
    {
        public class PersonalInformation
        {
            public string StudentNumber { get; set; }

            public string Username { get; set; }

            public string ProfilePictureUrl { get; set; }

            public string Fullname { get; set; }

            public string Email { get; set; }

            public string Birthdate { get; set; }

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
        
        public class Timetable
        {
            public Weekday Weekday { get; set; }

            public string Code { get; set; }

            public string Name { get; set; }
            
            public string TimeFrom { get; set; }
            
            public string TimeTo { get; set; }
            
            public string Teacher { get; set; }
            
            public string Classroom { get; set; }

            public bool? IsMandatory { get; set; }
        }

        public record Weekday(int Value, string Name);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.ObisApiModels
{
    public class StudentSemesterNotes
    {
        public class Response
        {
            public List<Lesson> Lessons { get; set; }

            public Lesson GetLessonByCodeAndName(string code, string name)
            {
                return Lessons.Find(x => 
                    string.Equals(x.LessonCodeFromName, code, StringComparison.OrdinalIgnoreCase) && 
                    string.Equals(x.LessonNameFromName, name, StringComparison.OrdinalIgnoreCase));
            }
        }
        
        public class Lesson
        {
            public string Name { get; set; }

            public string LessonCodeFromName => Name.Split(' ')[0];

            public string LessonNameFromName => Name.Split(' ', 2)[^1];

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

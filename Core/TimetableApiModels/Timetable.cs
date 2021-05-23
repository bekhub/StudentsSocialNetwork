using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Core.TimetableApiModels
{
    public class Timetable
    {
        public class Response
        {
            public Faculty Faculty { get; set; }
            
            public Department Department { get; set; }
            
            public List<Time> Timetable { get; set; }
            
            public string Teacher { get; set; }
            
            public string Type { get; set; }
        }
        
        public class Time
        {
            [JsonPropertyName("time_day")]
            public string TimeDay { get; set; }
            
            [JsonPropertyName("time_hour")]
            public string TimeHour { get; set; }
            
            public string Classroom { get; set; }
        }
    }
}

using System.Collections.Generic;
using Core.Interfaces;

namespace Core.Entities
{
    public class Department : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; }
        
        public string ShortName { get; set; }

        public int InstituteId { get; set; }
        public Institute Institute { get; set; }

        public ICollection<Student> Students { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}

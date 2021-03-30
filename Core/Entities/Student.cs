using System;
using Core.Interfaces;

namespace Core.Entities
{
    public class Student : BaseEntity, IAggregateRoot
    {
        public string StudentNumber { get; set; }

        public string StudentEmail { get; set; }

        public string StudentPassword { get; set; }

        public int? AdmissionYear { get; set; }

        public string BirthPlace { get; set; }
        
        public DateTime? BirthDate { get; set; }
        
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}

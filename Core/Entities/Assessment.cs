using Core.Interfaces;

namespace Core.Entities
{
    public class Assessment : BaseEntity, IAggregateRoot
    {
        public int Point { get; set; }

        public string Type { get; set; }

        public int StudentCourseId { get; set; }
        public StudentCourse StudentCourse { get; set; }
    }
}

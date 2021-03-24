using Core.Interfaces;

namespace Core.Entities
{
    public class Department : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; }
        
        public string ShortName { get; set; }

        public int InstituteId { get; set; }
        public Institute Institute { get; set; }
    }
}

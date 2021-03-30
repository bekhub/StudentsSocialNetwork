using System.Collections.Generic;
using Core.Interfaces;

namespace Core.Entities
{
    public class Institute : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; }

        public ICollection<Department> Departments { get; set; }
    }
}

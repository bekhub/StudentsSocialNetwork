using System.Collections.Generic;
using Core.Interfaces;

namespace Core.Entities
{
    public class Institute : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; }

        public int InstituteTypeId { get; set; }
        public InstituteType InstituteType { get; set; }

        public IEnumerable<Department> Departments { get; set; }
    }
}

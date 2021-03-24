using System.Collections.Generic;

namespace Core.Entities
{
    public class InstituteType : BaseEntity
    {
        public string Name { get; set; }

        public IEnumerable<Institute> Institutes { get; set; }
    }
}

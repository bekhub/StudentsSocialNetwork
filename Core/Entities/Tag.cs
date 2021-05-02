using System.Collections.Generic;

namespace Core.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }

        public List<PostTag> PostTags { get; set; }
    }
}

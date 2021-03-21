using System.Collections.Generic;

namespace Core
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        
        protected virtual object Actual => this;
        
        public List<BaseDomainEvent> Events = new();
    }
}

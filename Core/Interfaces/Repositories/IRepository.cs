using Ardalis.Specification;

namespace Core.Interfaces.Repositories
{
    public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot { }
    
    public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot { }
}

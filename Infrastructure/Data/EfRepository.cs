using Ardalis.Specification.EntityFrameworkCore;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class EfRepository<T> : RepositoryBase<T>, IRepository<T> where T : class, IAggregateRoot
    {
        private readonly SsnDbContext _ssnDbContext;

        public EfRepository(SsnDbContext ssnDbContext) : base(ssnDbContext)
        {
            _ssnDbContext = ssnDbContext;
        }
    }
}

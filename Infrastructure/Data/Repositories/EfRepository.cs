using Ardalis.Specification.EntityFrameworkCore;
using Core.Interfaces;

namespace Infrastructure.Data.Repositories
{
    public class EfRepository<T> : RepositoryBase<T>, IRepository<T> where T : class, IAggregateRoot
    {
        protected readonly SsnDbContext SsnDbContext;

        public EfRepository(SsnDbContext ssnDbContext) : base(ssnDbContext)
        {
            SsnDbContext = ssnDbContext;
        }
    }
}

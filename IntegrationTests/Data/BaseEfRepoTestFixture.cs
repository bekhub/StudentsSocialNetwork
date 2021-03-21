using Core.Interfaces;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace IntegrationTests.Data
{
    public abstract class BaseEfRepoTestFixture
    {
        protected SsnDbContext DbContext;

        protected static DbContextOptions<SsnDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<SsnDbContext>();
            builder.UseInMemoryDatabase("SsnBackend")
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        protected EfRepository<T> GetRepository<T>() where T : class, IAggregateRoot
        {
            var options = CreateNewContextOptions();
            var mockMediator = new Mock<IMediator>();

            DbContext = new SsnDbContext(options, mockMediator.Object);
            return new EfRepository<T>(DbContext);
        }
    }
}

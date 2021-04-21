using System;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Api.Helpers
{
    public class FireAndForgetHandler : IFireAndForgetHandler
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<FireAndForgetHandler> _logger;

        public FireAndForgetHandler(IServiceScopeFactory serviceScopeFactory, 
            ILogger<FireAndForgetHandler> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public void Execute<TService>(Func<TService, Task> serviceWork)
        {
            Task.Run(async () =>
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var service = scope.ServiceProvider.GetRequiredService<TService>();
                    await serviceWork(service);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "RegistrationService.SynchronizeStudentCourses");
                }
            });
        }
    }
}

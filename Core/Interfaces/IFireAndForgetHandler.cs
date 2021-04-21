using System;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IFireAndForgetHandler
    {
        void Execute<TService>(Func<TService, Task> serviceWork);
    }
}

using Core.Interfaces.Services;
using Infrastructure.Data;

namespace Api.Services
{
    public class AccountsService
    {
        private readonly IObisApiService _obisApiService;
        private readonly IEncryptionService _encryptionService;
        private readonly SsnDbContext _ssnDbContext;

        public AccountsService(IObisApiService obisApiService, IEncryptionService encryptionService, SsnDbContext ssnDbContext)
        {
            _obisApiService = obisApiService;
            _encryptionService = encryptionService;
            _ssnDbContext = ssnDbContext;
        }
        
        
    }
}

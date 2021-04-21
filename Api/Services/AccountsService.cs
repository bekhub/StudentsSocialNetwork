using Core.Interfaces.Services;
using Infrastructure.Data;

namespace Api.Services
{
    public class AccountsService
    {
        private readonly IRestApiService _restApiService;
        private readonly IEncryptionService _encryptionService;
        private readonly SsnDbContext _ssnDbContext;

        public AccountsService(IRestApiService restApiService, IEncryptionService encryptionService, SsnDbContext ssnDbContext)
        {
            _restApiService = restApiService;
            _encryptionService = encryptionService;
            _ssnDbContext = ssnDbContext;
        }
        
        
    }
}

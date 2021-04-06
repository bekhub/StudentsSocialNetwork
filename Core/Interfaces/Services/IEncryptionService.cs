using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IEncryptionService
    {
        string Encrypt(string text);
        
        Task<string> EncryptAsync(string text);
        
        string Decrypt(string cipherText);
        
        Task<string> DecryptAsync(string cipherText);
    }
}

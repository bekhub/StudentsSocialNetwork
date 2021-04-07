using System.IO;
using Microsoft.AspNetCore.Http;

namespace Api.Helpers.Extensions
{
    public static class FileExtensions
    {
        public static byte[] ToArray(this IFormFile file)
        {
            using var stream = new MemoryStream();
            file.CopyTo(stream);
            
            return stream.ToArray();
        }
    }
}

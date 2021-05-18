using System;
using System.IO;

namespace Common.Extensions
{
    public static class FileExtensions
    {
        private const int IMAGE_MAXIMUM_BYTES = 4_194_304;
        
        public static bool IsValidImage(this byte[] postedFile, string fileName)
        {
            return postedFile is {Length: > 0 and <= IMAGE_MAXIMUM_BYTES} && IsExtensionValid(fileName);
        }

        private static bool IsExtensionValid(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return false;
            var extension = Path.GetExtension(fileName);

            return string.Equals(extension, ".jpg", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(extension, ".png", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(extension, ".svg", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(extension, ".gif", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(extension, ".jpeg", StringComparison.OrdinalIgnoreCase);
        }
    }
}

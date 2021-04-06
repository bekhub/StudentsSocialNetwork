using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Api.Services
{
    public class WebFileSystem : IFileSystem
    {
        public async Task<bool> SavePictureAsync(string pictureName, byte[] picture, string folder = "")
        {
            if (!picture.IsValidImage(pictureName)) return false;
            
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/images", folder);
            var fullPath = Path.Combine(folderPath, pictureName);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            if (File.Exists(fullPath)) File.Delete(fullPath);

            await File.WriteAllBytesAsync(fullPath, picture);
            
            return true;
        }

        public void DeletePicture(string pictureName, string folder = "")
        {
            if (string.IsNullOrEmpty(pictureName)) return;
            
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), $@"wwwroot/images/{folder}", pictureName);
            if (File.Exists(fullPath)) File.Delete(fullPath);
        }
        
        public string MakePictureUrl(string pictureName, string folder = "")
        {
            return Path.Combine($@"/static/images/{folder}", pictureName);
        }

        public string GeneratePictureName(string pictureName)
        {
            return $"{DateTime.UtcNow.Ticks}{Path.GetExtension(pictureName)}";
        }

        public string GetPictureName(string pictureUrl)
        {
            if (string.IsNullOrEmpty(pictureUrl)) return string.Empty;
            
            var picName = pictureUrl.Split('/').Last();
            return picName;
        }
    }
    
    public static class ImageValidators
    {
        private const int IMAGE_MAXIMUM_BYTES = 4_194_304;

        public static bool IsValidImage(this byte[] postedFile, string fileName)
        {
            return postedFile != null && postedFile.Length > 0 && postedFile.Length <= IMAGE_MAXIMUM_BYTES && IsExtensionValid(fileName);
        }

        private static bool IsExtensionValid(string fileName)
        {
            var extension = Path.GetExtension(fileName);

            return string.Equals(extension, ".jpg", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(extension, ".png", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(extension, ".svg", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(extension, ".gif", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(extension, ".jpeg", StringComparison.OrdinalIgnoreCase);
        }
    }
}

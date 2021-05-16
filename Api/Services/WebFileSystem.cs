using System.IO;
using System.Threading.Tasks;
using Common.Extensions;
using Core.Interfaces;

namespace Api.Services
{
    public class WebFileSystem : IFileSystem
    {
        public async Task<string> SavePictureAsync(string pictureName, byte[] picture, string folder = "")
        {
            if (!picture.IsValidImage(pictureName)) return null;
            pictureName = pictureName.GeneratePictureName();
            
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/images", folder);
            var fullPath = Path.Combine(folderPath, pictureName);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            if (File.Exists(fullPath)) File.Delete(fullPath);

            await File.WriteAllBytesAsync(fullPath, picture);
            
            return Path.Combine($@"/static/images/{folder}", pictureName);
        }

        public void DeletePicture(string pictureUrl, string folder = "")
        {
            if (string.IsNullOrEmpty(pictureUrl)) return;
            
            var pictureName = Path.GetFileName(pictureUrl);
            
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), $@"wwwroot/images/{folder}", pictureName);
            if (File.Exists(fullPath)) File.Delete(fullPath);
        }
    }
}

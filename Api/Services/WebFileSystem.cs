using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Common.Extensions;
using Core.Interfaces;

namespace Api.Services
{
    public class WebFileSystem : IFileSystem
    {
        public string SavePicture(string pictureName, byte[] picture, string folder = "")
        {
            if (!picture.IsValidImage(pictureName)) return null;
            pictureName = pictureName.GeneratePictureName();
            
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/images", folder);
            var fullPath = Path.Combine(folderPath, pictureName);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            if (File.Exists(fullPath)) File.Delete(fullPath);

            File.WriteAllBytes(fullPath, picture);
            
            return Path.Combine($@"/static/images/{folder}", pictureName);
        }

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

        public Task DeletePictureAsync(string pictureUrl, string folder = "")
        {
            return Task.Run(() => DeletePicture(pictureUrl, folder));
        }

        public void DeletePictures(IEnumerable<string> pictureUrls, string folder = "")
        {
            pictureUrls = pictureUrls.ToList();
            if (!pictureUrls.Any()) return;
            
            var pictureNames = pictureUrls.Where(x => !string.IsNullOrEmpty(x)).Select(Path.GetFileName).ToList();
            
            var fullPaths = pictureNames
                .Select(x => Path.Combine(Directory.GetCurrentDirectory(), $@"wwwroot/images/{folder}", x))
                .ToList();
            fullPaths.ForEach(x =>
            {
                if (File.Exists(x))
                    File.Delete(x);
            });
        }

        public Task DeletePicturesAsync(IEnumerable<string> pictureUrls, string folder = "")
        {
            return Task.Run(() => DeletePictures(pictureUrls, folder));
        }
    }
}

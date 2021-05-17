using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Api.Configuration;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Common.Extensions;
using Core.Interfaces;
using Microsoft.Extensions.Options;

namespace Api.Services
{
    public class CloudinaryFileSystem : IFileSystem
    {
        private readonly Cloudinary _cloudinary;
        
        public CloudinaryFileSystem(IOptions<CloudinarySettings> cloudinaryConfig)
        {
            var cloudinarySettings = cloudinaryConfig.Value;
            var account = new Account(
                cloudinarySettings.CloudName, 
                cloudinarySettings.ApiKey,
                cloudinarySettings.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public string SavePicture(string pictureName, byte[] picture, string folder = "")
        {
            if (!picture.IsValidImage(pictureName)) return null;
            pictureName = pictureName.GeneratePictureName();
            
            using var stream = new MemoryStream(picture);
            var uploadParams = new ImageUploadParams
            {
                Folder = folder,
                PublicId = Path.GetFileNameWithoutExtension(pictureName),
                File = new FileDescription(pictureName, stream),
            };
            var uploadResult = _cloudinary.Upload(uploadParams);
            return uploadResult.Url.ToString();
        }

        public async Task<string> SavePictureAsync(string pictureName, byte[] picture, string folder = "")
        {
            if (!picture.IsValidImage(pictureName)) return null;
            pictureName = pictureName.GeneratePictureName();
            
            await using var stream = new MemoryStream(picture);
            var uploadParams = new ImageUploadParams
            {
                Folder = folder,
                PublicId = Path.GetFileNameWithoutExtension(pictureName),
                File = new FileDescription(pictureName, stream),
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.Url.ToString();
        }

        public void DeletePicture(string pictureUrl, string folder = "")
        {
            var pictureName = Path.GetFileNameWithoutExtension(pictureUrl);
            _cloudinary.Destroy(new DeletionParams(pictureName));
        }

        public Task DeletePictureAsync(string pictureUrl, string folder = "")
        {
            var pictureName = Path.GetFileNameWithoutExtension(pictureUrl);
            return _cloudinary.DestroyAsync(new DeletionParams(pictureName));
        }

        public void DeletePictures(IEnumerable<string> pictureUrls, string folder = "")
        {
            var pictureNames = pictureUrls.Select(Path.GetFileNameWithoutExtension).ToList();
            pictureNames.AsParallel().Select(x => _cloudinary.Destroy(new DeletionParams(x)));
        }

        public Task DeletePicturesAsync(IEnumerable<string> pictureUrls, string folder = "")
        {
            var pictureNames = pictureUrls.Select(Path.GetFileNameWithoutExtension).ToArray();
            pictureNames.AsParallel().Select(x => _cloudinary.Destroy(new DeletionParams(x)));
            return Task.CompletedTask;
        }
    }
}

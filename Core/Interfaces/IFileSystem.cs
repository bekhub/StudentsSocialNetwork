using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IFileSystem
    {
        Task<bool> SavePictureAsync(string pictureName, byte[] picture, string folder = "");

        void DeletePicture(string pictureName, string folder = "");
        
        string MakePictureUrl(string pictureName, string folder="");

        string GeneratePictureName(string pictureName);

        string GetPictureName(string pictureUrl);
    }
}

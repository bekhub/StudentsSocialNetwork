using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IFileSystem
    {
        /// <summary>
        /// Saves picture to implemented file system
        /// </summary>
        /// <param name="pictureName">picture name</param>
        /// <param name="picture">picture in byte array</param>
        /// <param name="folder">folder where the picture will be saved</param>
        /// <returns>picture url</returns>
        Task<string> SavePictureAsync(string pictureName, byte[] picture, string folder = "");

        /// <summary>
        /// Deletes picture from implemented file system
        /// </summary>
        /// <param name="pictureUrl">picture url</param>
        /// <param name="folder">folder where the picture was saved</param>
        void DeletePicture(string pictureUrl, string folder = "");
    }
}

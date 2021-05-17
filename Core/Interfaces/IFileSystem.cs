using System.Collections.Generic;
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
        string SavePicture(string pictureName, byte[] picture, string folder = "");
        
        /// <summary>
        /// Saves picture to implemented file system (asynchronous)
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
        
        /// <summary>
        /// Deletes picture from implemented file system (asynchronous)
        /// </summary>
        /// <param name="pictureUrl">picture url</param>
        /// <param name="folder">folder where the picture was saved</param>
        Task DeletePictureAsync(string pictureUrl, string folder = "");

        /// <summary>
        /// Deletes pictures from implemented file system
        /// </summary>
        /// <param name="pictureUrls">pictures urls</param>
        /// <param name="folder">folder where the pictures was saved</param>
        void DeletePictures(IEnumerable<string> pictureUrls, string folder = "");
        
        /// <summary>
        /// Deletes pictures from implemented file system (asynchronous)
        /// </summary>
        /// <param name="pictureUrls">pictures urls</param>
        /// <param name="folder">folder where the pictures was saved</param>
        Task DeletePicturesAsync(IEnumerable<string> pictureUrls, string folder = "");
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Helpers.Extensions;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Posts
{
    public class PostsService
    {
        private readonly SsnDbContext _context;
        private readonly IFileSystem _fileSystem;

        private const string FOLDER = "post_pictures";

        public PostsService(SsnDbContext context, IFileSystem fileSystem)
        {
            _context = context;
            _fileSystem = fileSystem;
        }

        public IQueryable<Post> GetAllData()
        {
            return _context.Posts
                .Where(x => x.IsActive)
                .Include(x => x.Pictures)
                .Include(x => x.Tags)
                .Include(x => x.User)
                .Include(x => x.PostLikes)
                .Include(x => x.Comments)
                    .ThenInclude(x => x.Replies)
                .AsNoTracking();
        }

        public Task<Post> ByIdAsync(int id, CancellationToken cancellationToken = new())
        {
            return _context.Posts
                .Include(x => x.Tags)
                .Include(x => x.Pictures)
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public List<Tag> GetTags(HashSet<string> tagSet)
        {
            if (tagSet == null || !tagSet.Any())
                return new List<Tag>();
            
            var tags = _context.Tags.Where(x => tagSet.Contains(x.Name)).ToList();
            var existingTags = tags.Select(x => x.Name).ToHashSet();
            var newTags = tagSet.Where(x => !existingTags.Contains(x)).Select(x => new Tag {Name = x});
            tags.AddRange(newTags);
            return tags;
        }

        public List<PostPicture> GetPictures(List<IFormFile> files)
        {
            if (files == null || !files.Any())
                return new List<PostPicture>();
            
            var urls = files
                .AsParallel()
                .Select(x => _fileSystem.SavePicture(x.FileName, x.ToArray(), FOLDER));
            
            return urls.Select(x => new PostPicture {Url = x}).ToList();
        }

        public void DeletePictures(IEnumerable<string> pictureUrls)
        {
            _fileSystem.DeletePictures(pictureUrls, FOLDER);
        }
    }
}

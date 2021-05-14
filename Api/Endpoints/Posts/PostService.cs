using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Api.Endpoints.Posts
{
    public class PostService
    {
        public async Task<Post> CreatePostAsync(
            string body, bool isActive, bool isDraft, string userId)
        {
            return new Post
            {
                Body = body,
                IsActive = isActive,
                IsDraft = isDraft,
                UserId = userId
            };
        }

        public async Task<bool> UserNotFound(string id)
        {
            // if()
            return false;
        }
    }
}
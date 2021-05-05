using System.Threading.Tasks;
using Core.Entities;

namespace Api.Endpoints.Posts
{
    public class PostService
    {
        private async Task<Post> CreatePostAsync(string body, Student student)
        {
            return new Post
            {
                Body = body,
                UserId = student.UserId
            };
        }
    }
}
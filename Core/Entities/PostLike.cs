namespace Core.Entities
{
    public class PostLike
    {
        public bool IsLiked { get; set; }
        
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}

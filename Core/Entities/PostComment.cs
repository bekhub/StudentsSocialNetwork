namespace Core.Entities
{
    public class PostComment
    {
        public int PostId { get; set; }
        public Post Post { get; set; }

        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}

namespace Core.Entities
{
    public class CommentReply
    {
        public int ReplyId { get; set; }
        public Comment Reply { get; set; }

        public int TargetId { get; set; }
        public Comment Target { get; set; }
    }
}

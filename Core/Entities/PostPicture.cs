namespace Core.Entities
{
    public class PostPicture : BaseEntity
    {
        public string Url { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}

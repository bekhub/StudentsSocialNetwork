namespace Api.Endpoints.Comments
{
    public class Request
    {
        public class Create
        {
            public int? PostId { get; set; }

            public int? TargetId { get; set; }
            
            public string Body { get; set; }
        }

        public record Delete(int Id);

        public record List(int PostId);

        public record Like(int Id);
    }
}

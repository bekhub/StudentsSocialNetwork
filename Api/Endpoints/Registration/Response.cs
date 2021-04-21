namespace Api.Endpoints.Registration
{
    public class Response
    {
        public class CheckStudent
        {
            public bool IsExist { get; set; }

            public string Message { get; set; }
        }

        public record VerifyEmail(bool IsVerified);
    }
}

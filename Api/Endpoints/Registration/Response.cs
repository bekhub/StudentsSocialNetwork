namespace Api.Endpoints.Registration
{
    public class Response
    {
        public record CheckStudent(bool IsExist, string Message);

        public record VerifyEmail(bool IsVerified);
    }
}

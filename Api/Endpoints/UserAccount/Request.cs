namespace Api.Endpoints.UserAccount
{
    public class Request
    {
        public record ChangePassword(string CurrentPassword, string NewPassword);
    }
}

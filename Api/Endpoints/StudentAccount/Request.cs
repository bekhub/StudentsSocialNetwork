namespace Api.Endpoints.StudentAccount
{
    public class Request
    {
        public record PersonalInformation(string UserId);

        public record LessonsAndMarks(string UserId);
    }
}

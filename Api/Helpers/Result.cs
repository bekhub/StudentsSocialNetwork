namespace Api.Helpers
{
    public record Result(string Message)
    {
        private const string USER_EXISTS = "User already registered";
        private const string USER_NOT_FOUND = "User not found";
        private const string REGISTER_ERROR = "Error during registering user";
        private const string REGISTER_SUCCESS = "User created";
        private const string EMAIL_REQUIRED = "Email required";
        private const string EMAIL_SENT = "Confirmation email sent";

        public static Result UserExists => new (USER_EXISTS);
        public static Result RegisterError => new (REGISTER_ERROR);
        public static Result RegisterSuccess => new (REGISTER_SUCCESS);
        public static Result UserNotFound => new(USER_NOT_FOUND);
        public static Result EmailRequired => new(EMAIL_REQUIRED);
        public static Result EmailSent => new(EMAIL_SENT);
    }
}

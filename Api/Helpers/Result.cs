using Api.Resources;

namespace Api.Helpers
{
    public record Result(string Message)
    {
        /// <summary>
        /// User already registered
        /// </summary>
        public static Result UserExists => new (DefaultResource.UserExists);
        /// <summary>
        /// Error during registering user
        /// </summary>
        public static Result RegisterError => new (DefaultResource.RegisterError);
        /// <summary>
        /// User created
        /// </summary>
        public static Result RegisterSuccess => new (DefaultResource.RegisterSuccess);
        /// <summary>
        /// User not found
        /// </summary>
        public static Result UserNotFound => new(DefaultResource.UserNotFound);
        /// <summary>
        /// Email required
        /// </summary>
        public static Result EmailRequired => new(DefaultResource.EmailRequired);
        /// <summary>
        /// Confirmation email sent
        /// </summary>
        public static Result EmailSent => new(DefaultResource.EmailSent);
        /// <summary>
        /// Password has been changed
        /// </summary>
        public static Result PasswordChanged => new(DefaultResource.PasswordChanged);
        /// <summary>
        /// Password has been updated
        /// </summary>
        public static Result PasswordUpdated => new(DefaultResource.PasswordChanged);
        /// <summary>
        /// Invalid password
        /// </summary>
        public static Result InvalidPassword => new(DefaultResource.InvalidPassword);
        /// <summary>
        /// Error while changing password
        /// </summary>
        public static Result PasswordError => new(DefaultResource.PasswordError);
    }
}

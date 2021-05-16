using Api.Resources;

namespace Api.Helpers
{
    public record Result(string Message)
    {
        private const string POST_CREATED = "New post created";
        private const string POST_NOT_CREATED = "New post couldn't created";
        private const string POST_ERROR = "Post wasn't saved";
        private const string NOT_AUTHORIZED = "First authorize, then create posts!";
        private const string POST_NOT_FOUND = "Post cannot be found";

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
        /// <summary>
        /// Post created
        /// </summary>
        public static Result PostCreated => new(POST_CREATED);
        /// <summary>
        /// Post not created
        /// </summary>
        public static Result PostNotCreated => new(POST_NOT_CREATED);
        /// <summary>
        /// Post not saved
        /// </summary>
        public static Result PostError => new(POST_ERROR);
        /// <summary>
        /// User didn't authorize
        /// </summary>
        public static Result NotAuthorized => new(NOT_AUTHORIZED);
        /// <summary>
        /// Post doesn't found
        /// </summary>
        public static Result PostNotFound => new(POST_NOT_FOUND);
    }
}

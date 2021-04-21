﻿namespace Api.Helpers
{
    public record Result(string Message)
    {
        private const string USER_EXISTS = "User already registered";
        private const string USER_NOT_FOUND = "User not found";
        private const string REGISTER_ERROR = "Error during registering user";
        private const string REGISTER_SUCCESS = "User created";
        private const string EMAIL_REQUIRED = "Email required";
        private const string EMAIL_SENT = "Confirmation email sent";

        /// <summary>
        /// User already registered
        /// </summary>
        public static Result UserExists => new (USER_EXISTS);
        /// <summary>
        /// Error during registering user
        /// </summary>
        public static Result RegisterError => new (REGISTER_ERROR);
        /// <summary>
        /// User created
        /// </summary>
        public static Result RegisterSuccess => new (REGISTER_SUCCESS);
        /// <summary>
        /// User not found
        /// </summary>
        public static Result UserNotFound => new(USER_NOT_FOUND);
        /// <summary>
        /// Email required
        /// </summary>
        public static Result EmailRequired => new(EMAIL_REQUIRED);
        /// <summary>
        /// Confirmation email sent
        /// </summary>
        public static Result EmailSent => new(EMAIL_SENT);
    }
}
namespace Api.Helpers
{
    public record Result(string Message)
    {
        /// <summary>
        /// Result containing message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Result(string message)</returns>
        public static Result From(string message) => new (message);
    }
}

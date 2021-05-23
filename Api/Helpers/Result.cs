using Microsoft.AspNetCore.Mvc;

namespace Api.Helpers
{
    public record Result(string Message)
    {
        /// <summary>
        /// Result containing message
        /// </summary>
        /// <param name="message">message to display</param>
        /// <returns>Result(string)</returns>
        public static Result From(string message) => new (message);

        /// <summary>
        /// Bad request object containing message
        /// </summary>
        /// <param name="message">message to display</param>
        /// <returns>BadRequestObjectResult(Result)</returns>
        public static BadRequestObjectResult BadRequest(string message) => new (From(message));

        /// <summary>
        /// Ok object containing message
        /// </summary>
        /// <param name="message">message to display</param>
        /// <returns>OkObjectResult(Result)</returns>
        public static OkObjectResult Ok(string message) => new (From(message));
    }
}

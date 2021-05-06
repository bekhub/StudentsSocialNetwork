using System.ComponentModel.DataAnnotations;

namespace Common
{
    public enum ApiResultStatusCode
    {
        [Display(Name = "Request completed")]
        Success = 0,

        [Display(Name = "An error occurred on the server")]
        ServerError = 1,

        [Display(Name = "The parameters sent are invalid")]
        BadRequest = 2,

        [Display(Name = "Not found")]
        NotFound = 3,

        [Display(Name = "The list is empty")]
        ListEmpty = 4,

        [Display(Name = "An error occurred while processing your request")]
        LogicError = 5,

        [Display(Name = "Authorization error")]
        UnAuthorized = 6,
    }
}

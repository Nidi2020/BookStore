using System.ComponentModel.DataAnnotations;

namespace BookStore.Common.Api;

public enum ApiResultStatusCode
{
    [Display(Name = "The operation was successful")]
    Success = 0,

    [Display(Name = "An error has occurred on the server")]
    ServerError = 1,

    [Display(Name = "The parameters sent are not valid")]
    BadRequest = 2,

    [Display(Name = "Not found")]
    NotFound = 3,

    [Display(Name = "The list is empty")]
    ListEmpty = 4,

    [Display(Name = "A processing error occurred")]
    LogicError = 5,

    [Display(Name = "Authentication error")]
    UnAuthorized = 6
}


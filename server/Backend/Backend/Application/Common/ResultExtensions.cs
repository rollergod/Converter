namespace Backend.Application.Common
{
    public static class ResultExtensions
    {
        public static T Match<T>(this Result result, Func<T> OnSuccess, Func<Error, T> OnFailure)
        {
            return result.IsSuccess 
                ? OnSuccess() 
                : OnFailure(result.Error);
        }

        public static IResult ToProblemDetails(this Result result)
        {
            return Results.Problem(
                statusCode: GetCode(result.Error.ErrorType),
                title: result.Error.Code,
                detail: result.Error.Description
            );
        }

        static int GetCode(ErrorType error) =>
            error switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Auth => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };
    }
}

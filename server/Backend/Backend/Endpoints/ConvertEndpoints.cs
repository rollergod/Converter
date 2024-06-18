using Backend.Application.Common;
using Backend.Application.Contracts.Request;
using Backend.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Endpoints
{
    public static class ConvertEndpoints
    {
        public static void ConvertEndpointsMap(this IEndpointRouteBuilder app)
        {
            app.MapPost("/convert", [Authorize] async ([FromBody] ConvertRequest request, IAccountService accountService) =>
            {
                var result = await accountService.Convert(request.AccountId);

                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            }).WithTags("Convert");
        }
    }
}

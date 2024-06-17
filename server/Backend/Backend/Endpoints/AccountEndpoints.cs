using Backend.Application.Common;
using Backend.Application.Contracts.Request;
using Backend.Application.Contracts.Response;
using Backend.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Endpoints
{
    public static class AccountEndpoints
    {
        public static void MapAccountEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/Account",[Authorize] async (int userId, IAccountService accountService) =>
            {
                var accounts = await accountService.GetByUserId(userId);

                return accounts;
            });

            app.MapGet("/TransferAccounts", [Authorize] async (int userId, IAccountService accountService) =>
            {
                var accounts = await accountService.GetAccountsForTransfer(userId);

                return new TransferAccountsReponse { CurrentUserAccounts = accounts.Item1, TransferAccounts = accounts.Item2 };
            });

            app.MapPost("/Account", [Authorize] async ([FromBody] CreateAccountRequest request, IAccountService accountService) =>
            {
                var result = await accountService.CreateAsync(request);

                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            });

            app.MapPost("/Convert", [Authorize] async ([FromBody] ConvertRequest request, IAccountService accountService) =>
            {
                var result = await accountService.Convert(request.AccountId);

                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            });
        }
    }
}

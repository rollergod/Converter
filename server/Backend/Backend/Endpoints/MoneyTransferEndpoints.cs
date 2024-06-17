using Backend.Application.Common;
using Backend.Application.Contracts.Request;
using Backend.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Endpoints
{
    public static class MoneyTransferEndpoints
    {
        public static void MapMoneyTransferEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/TransferMoney", [Authorize] async ([FromBody] MoneyTransferRequest request, IMoneyTransferService moneyTransferService) =>
            {
                var result = await moneyTransferService.TransferToPerson(
                    request.fromAccountId, 
                    request.toAccountId,
                    request.money
                );

                return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
            });

            app.MapGet("/TransferMoney", [Authorize] async (
                [FromQuery] int userId,
                [FromQuery] DateTime startDate,
                [FromQuery] DateTime endDate,
                [FromQuery] string accountIds,
                [FromQuery] string currencyIds,
                IMoneyTransferService moneyTransferService) =>
            {
                var accountIdsList = string.IsNullOrEmpty(accountIds) ? new List<string>() : accountIds.Split(',').ToList();
                var currencyIdsList = string.IsNullOrEmpty(currencyIds) ? new List<string>() :  currencyIds.Split(',').ToList();

                var request = new MoneyTransferHistoryRequest
                {
                    UserId = userId,
                    EndDate = endDate.Date.AddDays(1).AddMilliseconds(-1),
                    StartDate = startDate.Date,
                    AccountIds = accountIdsList,
                    CurrencyIds = currencyIdsList,
                };

                var result = await moneyTransferService.GetTransferHistory(request);

                return Results.Ok(result);
            });

            app.MapGet("/TransferMoneyFilters", [Authorize] async (int userId, IMoneyTransferService moneyTransferService) =>
            {
                var response = await moneyTransferService.GetFilters(userId);

                return response;
            });
        }
    }
}

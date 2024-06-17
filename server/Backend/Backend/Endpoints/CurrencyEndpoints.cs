using Backend.Application.Common;
using Backend.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Endpoints
{
    public static class CurrencyEndpoints
    {
        public static void MapCurrencyEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/Currencies", [Authorize] async (ICurrencyService currencyService) =>
            {
                var currencies = await currencyService.GetCurrencies();

                return currencies;
            });

            app.MapPost("/Currencies",  [Authorize] async ([FromBody] string name, ICurrencyService currencyService) =>
            {
                var result = await currencyService.Create(name);

                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            });
        }
    }
}

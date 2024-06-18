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
            app.MapGet("/currencies", [Authorize] async (ICurrencyService currencyService) =>
            {
                var currencies = await currencyService.GetCurrencies();

                return currencies;
            }).WithTags("Currencies");

            app.MapPost("/currencies",  [Authorize] async ([FromBody] string name, ICurrencyService currencyService) =>
            {
                var result = await currencyService.Create(name);

                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            }).WithTags("Currencies");
        }
    }
}

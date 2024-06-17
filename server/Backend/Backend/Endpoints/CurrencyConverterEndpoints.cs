using Backend.Application.Common;
using Backend.Application.Contracts.Request;
using Backend.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Endpoints
{
    public static class CurrencyConverterEndpoints
    {
        public static void MapCurrencyConverterEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/Coefficients", [Authorize] async ([FromBody] CreateCurrencyConverter request, ICoefficientService coefficientService) =>
            {
                var result = await coefficientService.CreateCoefficientAsync(request);
                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            });

            app.MapGet("/Coefficients", [Authorize] async (ICoefficientService coefficientService) =>
            {
                var currencyConverterDtos = await coefficientService.GetCurrencyConvertersAsync();

                return currencyConverterDtos;
            });
        }
    }
}

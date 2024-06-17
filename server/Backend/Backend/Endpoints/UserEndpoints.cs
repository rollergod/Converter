using Backend.Application.Common;
using Backend.Application.Contracts.Request;
using Backend.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/Register", async ([FromBody] RegisterRequest request, IUserService userService) =>
            {
                var result = await userService.Register(request.userName, request.password);

                return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
            });

            app.MapPost("/Login", async ([FromBody] LoginRequest request, IUserService userService, HttpContext context) =>
            {
                var result = await userService.Login(request.userName, request.password);

                if (result.IsSuccess)
                {
                    context.Response.Cookies.Append("token", result.Value.Token, new CookieOptions
                    {
                       HttpOnly = true,
                       Secure = true,
                       SameSite = SameSiteMode.None,
                       Expires = DateTime.UtcNow.AddDays(1),
                    });
                }

                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            });

            app.MapPost("/Logout", (HttpContext context, IUserService userService) =>
            {
                if (context.Request.Cookies.ContainsKey("token"))
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None
                    };

                    context.Response.Cookies.Delete("token", cookieOptions);
                }

                return Results.Ok();
            });
        }
    }
}

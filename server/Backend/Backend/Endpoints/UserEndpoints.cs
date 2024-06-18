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
            app.MapPost("/auth/register", async ([FromBody] RegisterRequest request, IUserService userService) =>
            {
                var result = await userService.Register(request.userName, request.password);

                return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
            });

            app.MapPost("/auth/login", async ([FromBody] LoginRequest request, IUserService userService, HttpContext context) =>
            {
                var result = await userService.Login(request.userName, request.password);

                if (result.IsSuccess)
                {
                    SetCookies(context, result.Value.Token, result.Value.RefreshToken);
                    return Results.Ok(result.Value);
                }

                return result.ToProblemDetails();
            });

            app.MapPost("/auth/logout", (HttpContext context, IUserService userService) =>
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

                if (context.Request.Cookies.ContainsKey("refreshToken"))
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None
                    };

                    context.Response.Cookies.Delete("refreshToken", cookieOptions);
                }

                return Results.Ok();
            });

            app.MapPost("/auth/refresh", async (HttpContext context, IUserService userService) =>
            {
                var acccessToken = context.Request.Cookies["token"];
                var refreshToken = context.Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(acccessToken) || string.IsNullOrEmpty(refreshToken))
                {
                    return Results.Problem(statusCode: 401);
                }

                var result = await userService.RefreshToken(acccessToken, refreshToken);

                if (result.IsSuccess)
                {
                    SetCookies(context, result.Value.AccessToken, result.Value.RefreshToken);
                    return Results.Ok(result.Value);
                }

                return result.ToProblemDetails();
            });
        }
        private static void SetCookies(HttpContext context, string accessToken, string refreshToken)
        {
            context.Response.Cookies.Append("token", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(1),
            });

            context.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(1),
            });
        }
    }
}

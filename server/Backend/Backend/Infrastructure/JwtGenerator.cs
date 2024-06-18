using Backend.Application.Common;
using Backend.Application.Contracts.DTO;
using Backend.Application.Interfaces;
using Backend.Core.Errors;
using Backend.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Infrastructure
{
    public class JwtOptions
    {
        public string SecretKey { get; set; }
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
    }
    public class JwtGenerator(IOptions<JwtOptions> options) : IJwtGenerator
    {
        private readonly JwtOptions _options = options.Value;

        public TokenDto GenerateWithRefreshToken(User user)
        {
            Claim[] claims = [new("userId", user.Id.ToString())];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.Now.AddSeconds(_options.AccessTokenExpiration)
            );

            return new TokenDto
            {
                RefreshTokenExpiration = DateTime.UtcNow.AddHours(_options.RefreshTokenExpiration),
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = CreateRefreshToken()
            };
        }

        public Result<string> GetUserIdFromAccessToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var claimUserId = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "userId");

            if(claimUserId == null)
            {
                return Result.Failure<string>(AuthError.ClaimUserIdNotFound);
            }

            return Result.Success<string>(claimUserId.Value); ;
        }

        private string CreateRefreshToken()
        {
            var numberByte = new byte[32];

            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }
    }
}

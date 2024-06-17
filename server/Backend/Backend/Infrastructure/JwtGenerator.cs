using Backend.Application.Interfaces;
using Backend.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Infrastructure
{
    public class JwtOptions
    {
        public string SecretKey { get; set; }
        public int ExpireHours { get; set; }
    }
    public class JwtGenerator(IOptions<JwtOptions> options) : IJwtGenerator
    {
        private readonly JwtOptions _options = options.Value;

        public string Generate(User user)
        {
            Claim[] claims = [new("userId", user.Id.ToString())]; 

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                claims: claims,
                /*алгоритм*/ signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_options.ExpireHours)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

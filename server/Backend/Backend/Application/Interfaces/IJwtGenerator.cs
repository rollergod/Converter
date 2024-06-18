using Backend.Application.Common;
using Backend.Application.Contracts.DTO;
using Backend.Core.Models;

namespace Backend.Application.Interfaces
{
    public interface IJwtGenerator
    {
        TokenDto GenerateWithRefreshToken(User user);
        Result<string> GetUserIdFromAccessToken(string token);
    }
}

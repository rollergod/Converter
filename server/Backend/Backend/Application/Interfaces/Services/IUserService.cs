using Backend.Application.Common;
using Backend.Application.Contracts.DTO;

namespace Backend.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<Result> Register(string userName, string password);
        Task<Result<UserDto>> Login(string userName, string password);
        Task<Result<TokenDto>> RefreshToken(string accessToken, string refreshToken);
    }
}

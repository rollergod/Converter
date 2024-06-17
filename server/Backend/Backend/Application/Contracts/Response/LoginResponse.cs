using Backend.Application.Contracts.DTO;
using Backend.Core.Models;

namespace Backend.Application.Contracts.Response
{
    public record LoginResponse(string token, UserDto user);
}

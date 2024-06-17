using Backend.Core.Models;

namespace Backend.Application.Interfaces
{
    public interface IJwtGenerator
    {
        string Generate(User user);
    }
}

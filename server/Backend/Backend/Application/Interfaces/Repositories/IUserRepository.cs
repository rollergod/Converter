using Backend.Core.Models;

namespace Backend.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task Create(string userName, string hashedPassword);
        Task<User> GetByUserName(string userName); 
    }
}

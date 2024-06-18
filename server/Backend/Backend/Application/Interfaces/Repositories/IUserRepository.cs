using Backend.Core.Models;

namespace Backend.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task Create(string userName, string hashedPassword);
        Task Create(User user);
        Task<User> GetByUserName(string userName);
        Task<User> GetById(int id);
    }
}

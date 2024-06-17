using Backend.Application.Interfaces.Repositories;
using Backend.Core.Models;
using Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Repositories
{
    public class UserRepository(AppDbContext db) : IUserRepository
    {
        private readonly AppDbContext db = db;
        public async Task Create(string userName, string hashedPassword)
        {
            await db.Users.AddAsync(
            new User
                {
                    UserName = userName,
                    HashedPassword = hashedPassword
                }
            );

            await db.SaveChangesAsync();
        }

        public async Task<User> GetByUserName(string userName)
        {
            //если не нашел?
            var user = await db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserName == userName);

            return user;
        }
    }
}

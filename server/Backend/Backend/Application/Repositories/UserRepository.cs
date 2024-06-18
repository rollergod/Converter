using Backend.Application.Interfaces.Repositories;
using Backend.Core.Models;
using Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Repositories
{
    public class UserRepository(AppDbContext db) 
        : IUserRepository
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

        //v2
        public async Task Create(User user)
        {
            if(user.Id > 0)
            {
                db.Users.Update(user);
            }
            else
            {
                await db.Users.AddAsync(user);
            }

            await db.SaveChangesAsync();
        }


        public async Task<User> GetById(int id)
        {
            var user = await db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return user;
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

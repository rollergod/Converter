using Backend.Application.Interfaces;

namespace Backend.Infrastructure
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Generate(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }

        public bool Verify(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
        }
    }
}

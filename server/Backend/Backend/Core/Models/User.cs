namespace Backend.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiryRefreshTokenTime { get; set; }
    }
}

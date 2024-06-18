namespace Backend.Application.Contracts.DTO
{
    public class TokenDto
    {
        public DateTime RefreshTokenExpiration { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}

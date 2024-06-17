namespace Backend.Application.Contracts.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public List<AccountDto> Accounts { get; set; }
        internal string Token { get; set; }
    }
}

namespace Backend.Application.Contracts.DTO
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstCurrencyName { get; set; }
        public string SecondCurrencyName { get; set; }
        public decimal Balance { get; set; }
        public bool IsFirstCurrencyMain { get; set; }
    }
}

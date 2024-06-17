namespace Backend.Application.Contracts.DTO
{
    public class CurrencyConverterDto
    {
        public int Id { get; set; }
        public string FromCurrencyName { get; set; }
        public string ToCurrencyName { get; set; }
        public double Coefficient { get; set; }
    }
}

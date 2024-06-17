namespace Backend.Core.Models
{
    public class CurrencyConverter
    {
        public int Id { get; set; }
        public int FromCurrencyId { get; set; }
        public int ToCurrencyId { get; set; }
        public double Coefficient { get; set; } 

        public Currency FromCurrency { get; set; }
        public Currency ToCurrency { get; set; }
    }
}

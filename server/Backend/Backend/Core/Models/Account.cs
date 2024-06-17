namespace Backend.Core.Models
{
    public class Account
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FirstCurrencyId { get; set; }
        public int SecondCurrencyId { get; set; }
        public int MainCurrencyId { get; set; }
        public decimal Balance { get; set; }
        public string Name { get; set; }

        public User User { get; set; }
        public Currency FirstCurrency { get; set; }
        public Currency SecondCurrency { get; set; }
        public Currency MainCurrency { get; set; }

        public List<Transfer> TransfersFrom { get; set; }
        public List<Transfer> TransfersTo { get; set; }
    }
}

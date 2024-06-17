namespace Backend.Core.Models
{
    public class Transfer
    {
        public int Id { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransferDate { get; set; }
        public Account FromAccount { get; set; }
        public Account ToAccount { get; set; }
    }
}

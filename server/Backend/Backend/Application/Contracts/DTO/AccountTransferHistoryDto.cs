namespace Backend.Application.Contracts.DTO
{
    public class AccountTransferHistoryDto
    {
        public decimal Amount { get; set; }
        public DateTime TransferedDate { get; set; }
        public string ToAccountName { get; set; }
        public string FromAccountName { get; set; }
    }
}

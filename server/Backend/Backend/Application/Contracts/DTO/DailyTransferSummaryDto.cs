namespace Backend.Application.Contracts.DTO
{
    public class DailyTransferSummaryDto
    {
        public DateTime TransferedDate { get; set; }
        public decimal SpentAmount { get; set; }
        public decimal ReceivedAmount { get; set; }
    }
}

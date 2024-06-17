using Backend.Application.Contracts.DTO;

namespace Backend.Application.Contracts.Response
{
    public class AccountTransferHistoryResponse
    {
        public List<AccountTransferHistoryDto> TransferHistoryFromAccount { get; set; }
        public List<AccountTransferHistoryDto> TransferHistoryToAccount { get; set; }
    }
}

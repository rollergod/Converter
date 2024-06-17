using Backend.Application.Contracts.DTO;

namespace Backend.Application.Contracts.Response
{
    public class TransferAccountsReponse
    {
        public List<AccountDto> CurrentUserAccounts { get; set; }
        public List<TransferAccountDto> TransferAccounts { get; set; }
    }
}

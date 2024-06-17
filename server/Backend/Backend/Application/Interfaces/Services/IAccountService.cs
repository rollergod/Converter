using Backend.Application.Common;
using Backend.Application.Contracts.DTO;
using Backend.Application.Contracts.Request;

namespace Backend.Application.Interfaces.Services
{
    public interface IAccountService
    {
        public Task<List<AccountDto>> GetByUserId(int userId);
        public Task<(List<AccountDto>, List<TransferAccountDto>)> GetAccountsForTransfer(int currentUserId);
        public Task<Result<AccountDto>> CreateAsync(CreateAccountRequest request);
        public Task<Result<ConvertDto>> Convert(int accountId);
    }
}

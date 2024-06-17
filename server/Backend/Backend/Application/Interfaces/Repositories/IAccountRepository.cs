using Backend.Application.Contracts.Request;
using Backend.Core.Models;
using System.Data;
using System.Data.Common;

namespace Backend.Application.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> GetAccountByIdAsync(int id);
        Task<List<Account>> GetAccountsByUserIdWithTransferHistory(MoneyTransferHistoryRequest queryParams);
        Task<List<Account>> GetAccountsByUserId(int userId);
        Task<List<Account>> GetAllAccountsAsync();
        Task Create(Account account);
        Task ChangeBalance(Account account, decimal balance);

        DbTransaction BeginTransaction(IsolationLevel isolationLevel);
    }
}

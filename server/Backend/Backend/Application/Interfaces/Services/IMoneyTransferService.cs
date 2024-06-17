using Backend.Application.Common;
using Backend.Application.Contracts.DTO;
using Backend.Application.Contracts.Request;
using Backend.Application.Contracts.Response;

namespace Backend.Application.Interfaces.Services
{
    public interface IMoneyTransferService
    {
        Task<Result> TransferToPerson(int accountId, int toAccountId, decimal money);
        Task<List<DailyTransferSummaryDto>> GetTransferHistory(MoneyTransferHistoryRequest request);
        Task<MoneyTransferFilterResponse> GetFilters(int userId);
    }
}
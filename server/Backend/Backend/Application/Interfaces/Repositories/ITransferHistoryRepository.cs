using Backend.Core.Models;

namespace Backend.Application.Interfaces.Repositories
{
    public interface ITransferHistoryRepository
    {
        Task<List<Transfer>> GetTransfersFromAccount(int accountId);
        Task<List<Transfer>> GetTransfersToAccount(int accountId);
        Task Create(Transfer transfer);
    }
}

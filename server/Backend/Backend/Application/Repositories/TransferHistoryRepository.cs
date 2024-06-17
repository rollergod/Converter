using Backend.Application.Interfaces.Repositories;
using Backend.Core.Models;
using Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Repositories
{
    public class TransferHistoryRepository(AppDbContext db) 
        : ITransferHistoryRepository
    {
        private readonly AppDbContext db = db;

        public async Task Create(Transfer transfer)
        {
            await db.Transfers.AddAsync(transfer);
            await db.SaveChangesAsync();
        }
        public async Task<List<Transfer>> GetTransfersFromAccount(int accountId)
        {
            var transfers = await db.Transfers
                .AsNoTracking()
                .Where(x => x.FromAccountId == accountId)
                .Include(x => x.FromAccount)
                .ToListAsync();

            return transfers;
        }

        public async Task<List<Transfer>> GetTransfersToAccount(int accountId)
        {
            var transfers = await db.Transfers
                .AsNoTracking()
                .Where(x => x.ToAccountId == accountId)
                .Include(x => x.ToAccount)
                .ToListAsync();

            return transfers;
        }
    }
}

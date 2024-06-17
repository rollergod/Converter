using Backend.Application.Contracts.DTO;
using Backend.Application.Contracts.Request;
using Backend.Application.Interfaces.Repositories;
using Backend.Core.Models;
using Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;

namespace Backend.Application.Repositories
{
    public class AccountRepository(AppDbContext db)
        : IAccountRepository
    {
        private readonly AppDbContext db = db;
        public async Task<List<Account>> GetAccountsByUserId(int userId)
        {
            var accounts = await db.Accounts
                   .AsNoTracking()
                   .Where(x => x.UserId == userId)
                   .Include(x => x.FirstCurrency)
                   .Include(x => x.SecondCurrency)
                   .ToListAsync();

            return accounts;
        }

        public async Task Create(Account account)
        {
            if (account.Id > 0)
            {
                db.Accounts.Update(account);
                await db.SaveChangesAsync();
            }
            else
            {
                await db.Accounts.AddAsync(account);
                await db.SaveChangesAsync();
            }
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            var accounts = await db.Accounts
                   .AsNoTracking()
                   .Include(x => x.User)
                   .Include(x => x.FirstCurrency)
                   .Include(x => x.SecondCurrency)
                   .ToListAsync();

            return accounts;
        }

        public async Task<Account> GetAccountByIdAsync(int id)
        {
            var account = await db.Accounts
                   .AsNoTracking()
                   .Include(x => x.FirstCurrency)
                   .Include(x => x.SecondCurrency)
                   .FirstOrDefaultAsync(x => x.Id == id);

            return account;
        }

        public async Task<List<Account>> GetAccountsByUserIdWithTransferHistory(MoneyTransferHistoryRequest queryParams)
        {
            var query = db.Accounts
                .AsNoTracking()
                .Include(x => x.MainCurrency)
                .Where(x => x.UserId == queryParams.UserId);

            if (queryParams.AccountIds != null && queryParams.AccountIds.Any())
            {
                query = query.Where(x => queryParams.AccountIds.Contains(x.Name));
            }

            if (queryParams.CurrencyIds != null && queryParams.CurrencyIds.Any())
            {
                query = query.Where(x => queryParams.CurrencyIds.Contains(x.MainCurrency.Name));
            }

            query = query
            .Include(a => a.TransfersFrom
                .Where(t =>
                    t.TransferDate >= queryParams.StartDate &&
                    t.TransferDate <= queryParams.EndDate))
            .Include(a => a.TransfersTo
                .Where(t =>
                    t.TransferDate >= queryParams.StartDate &&
                    t.TransferDate <= queryParams.EndDate));

            var accounts = await query.ToListAsync();

            return accounts;
        }

        public async Task ChangeBalance(Account account, decimal balance)
        {
            account.Balance += balance;
            db.Entry(account).State = EntityState.Modified;
            db.Accounts.Update(account);
            await db.SaveChangesAsync();
        }

        public DbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            var transaction = db.Database.BeginTransaction(isolationLevel);

            return transaction.GetDbTransaction();
        }
    }
}

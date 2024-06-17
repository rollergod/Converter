using Backend.Application.Interfaces.Repositories;
using Backend.Core.Models;
using Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Repositories
{
    public class CurrencyRepository(AppDbContext db)
        : ICurrencyRepository
    {
        private readonly AppDbContext db = db;

        public async Task<Currency> Create(Currency currency)
        {
            await db.Currencies.AddAsync(currency);
            await db.SaveChangesAsync();

            return currency;
        }

        public async Task<Currency> GetByIdAsync(int id)
        {
            var currency = await db.Currencies.FirstOrDefaultAsync(x => x.Id == id);

            return currency;
        }

        public async Task<Currency> GetByNameAsync(string name)
        {
            var currency = await db.Currencies.FirstOrDefaultAsync(x => x.Name == name);

            return currency;
        }

        public async Task<List<Currency>> GetCurrencies()
        {
            var currencies = await db.Currencies
                .AsNoTracking()
                .ToListAsync();

            return currencies;
        }
    }
}

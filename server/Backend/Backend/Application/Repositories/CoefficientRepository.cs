using Backend.Application.Interfaces.Repositories;
using Backend.Core.Models;
using Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Repositories
{
    public class CoefficientRepository(AppDbContext db)
        : ICoefficientRepository
    {
        public async Task<int> CreateAsync(CurrencyConverter currencyConverter)
        {
            if(currencyConverter.Id > 0)
            {
                db.CurrencyConverters.Update(currencyConverter);
                await db.SaveChangesAsync();
            }
            else
            {
                await db.CurrencyConverters.AddAsync(currencyConverter);
                await db.SaveChangesAsync();
            }

            return currencyConverter.Id;
        }

        public async Task<List<CurrencyConverter>> GetAllAsync()
        {
            var currencyConverters = await db.CurrencyConverters
                .AsNoTracking()
                .Include(x => x.FromCurrency)
                .Include(x => x.ToCurrency)
                .ToListAsync();

            return currencyConverters;
        }
        public async Task<CurrencyConverter> GetByIdAsync(int id)
        {
            var currencyConverter = await db.CurrencyConverters.FirstOrDefaultAsync(x => x.Id == id);

            return currencyConverter;
        }

        public async Task<CurrencyConverter> GetByFromAndToIds(int fromId, int toId)
        {
            var currencyConverter = await db.CurrencyConverters
                .FirstOrDefaultAsync(x => x.FromCurrencyId == fromId && x.ToCurrencyId == toId);

            return currencyConverter;
        }
    }
}

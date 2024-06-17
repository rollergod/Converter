using Backend.Core.Models;

namespace Backend.Application.Interfaces.Repositories
{
    public interface ICurrencyRepository
    {
        Task<Currency> Create(Currency currency);
        Task<Currency> GetByNameAsync(string name);
        Task<Currency> GetByIdAsync(int id);
        Task<List<Currency>> GetCurrencies();
    }
}

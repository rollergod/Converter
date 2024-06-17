using Backend.Core.Models;

namespace Backend.Application.Interfaces.Repositories
{
    public interface ICoefficientRepository
    {
        public Task<List<CurrencyConverter>> GetAllAsync();
        public Task<CurrencyConverter> GetByIdAsync(int id);
        public Task<CurrencyConverter> GetByFromAndToIds(int fromId, int toId);
        public Task<int> CreateAsync(CurrencyConverter currencyConverter);
    }
}

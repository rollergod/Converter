using Backend.Application.Common;
using Backend.Core.Models;

namespace Backend.Application.Interfaces.Services
{
    public interface ICurrencyService
    {
        public Task<List<Currency>> GetCurrencies();
        public Task<Result<Currency>> Create(string name);
    }
}

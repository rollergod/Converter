using Backend.Application.Common;
using Backend.Application.Interfaces.Repositories;
using Backend.Application.Interfaces.Services;
using Backend.Core.Errors;
using Backend.Core.Models;

namespace Backend.Application.Services
{
    public class CurrencyService(ICurrencyRepository _currencyRepository)
        : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository = _currencyRepository;
        public async Task<List<Currency>> GetCurrencies()
        {
            var currencies = await _currencyRepository.GetCurrencies();

            return currencies;
        }

        public async Task<Result<Currency>> Create(string name)
        {
            var currency = await _currencyRepository.GetByNameAsync(name);

            if(currency != null)
            {
                return Result.Failure<Currency>(CurrencyError.Exist);
            }

            currency = new Currency { Name = name };
            return Result.Success<Currency>(await _currencyRepository.Create(currency));
        }
    }
}

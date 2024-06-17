using Backend.Application.Common;
using Backend.Application.Contracts.DTO;
using Backend.Application.Contracts.Request;
using Backend.Application.Interfaces.Repositories;
using Backend.Application.Interfaces.Services;
using Backend.Core.Errors;
using Backend.Core.Models;

namespace Backend.Application.Services
{
    public class CoefficientService(ICoefficientRepository coefficientRepository, ICurrencyRepository currencyRepository)
        : ICoefficientService
    {
        private readonly ICoefficientRepository _coefficientRepository = coefficientRepository;
        private readonly ICurrencyRepository _currencyRepository = currencyRepository;

        public async Task<Result> ChangeCoefficient(int id, double coefficient)
        {
            var currencyConverter = await _coefficientRepository.GetByIdAsync(id);

            if(currencyConverter is null)
            {
                return Result.Failure(CurrencyConverterError.NotFound);
            }

            currencyConverter.Coefficient = coefficient;

            await _coefficientRepository.CreateAsync(currencyConverter);
            return Result.Success();
        }
        public async Task<Result<CurrencyConverterDto>> CreateCoefficientAsync(CreateCurrencyConverter request)
        {
            if(request.fromId == request.toId)
            {
                return Result.Failure<CurrencyConverterDto>(CurrencyConverterError.FromAndToIdsEquals);
            }

            var fromCurrency = await _currencyRepository.GetByIdAsync(request.fromId);
            var toCurrency = await _currencyRepository.GetByIdAsync(request.toId);

            if(fromCurrency == null && toCurrency == null)
            {
                return Result.Failure<CurrencyConverterDto>(CurrencyError.NotFound);
            }

            var currentCurrency = await _coefficientRepository.GetByFromAndToIds(request.fromId, request.toId);
            
            if(currentCurrency != null)
            {
                return Result.Failure<CurrencyConverterDto>(CurrencyConverterError.Exist);
            }

            currentCurrency = new CurrencyConverter
            {
                FromCurrencyId = request.fromId,
                ToCurrencyId = request.toId,
                Coefficient = request.coefficient
            };

            await _coefficientRepository.CreateAsync(currentCurrency);

            return Result.Success<CurrencyConverterDto>(new CurrencyConverterDto
            {
                Id = currentCurrency.Id,
                Coefficient = currentCurrency.Coefficient,
                FromCurrencyName = fromCurrency.Name,
                ToCurrencyName = toCurrency.Name
            });
        }

        public async Task<List<CurrencyConverterDto>> GetCurrencyConvertersAsync()
        {
            var currencyConverters = await _coefficientRepository.GetAllAsync();

            return currencyConverters
                .Select(x => new CurrencyConverterDto
                {
                    Id = x.Id,
                    Coefficient = x.Coefficient,
                    FromCurrencyName = x.FromCurrency.Name,
                    ToCurrencyName = x.ToCurrency.Name
                })
                .ToList();
        }
    }
}

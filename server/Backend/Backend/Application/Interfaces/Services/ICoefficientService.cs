using Backend.Application.Common;
using Backend.Application.Contracts.DTO;
using Backend.Application.Contracts.Request;

namespace Backend.Application.Interfaces.Services
{
    public interface ICoefficientService
    {
        public Task<List<CurrencyConverterDto>> GetCurrencyConvertersAsync();
        public Task<Result> ChangeCoefficient(int id, double coefficient);
        public Task<Result<CurrencyConverterDto>> CreateCoefficientAsync(CreateCurrencyConverter request);
    }
}

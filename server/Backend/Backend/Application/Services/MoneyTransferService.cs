using Backend.Application.Common;
using Backend.Application.Contracts.DTO;
using Backend.Application.Contracts.Request;
using Backend.Application.Contracts.Response;
using Backend.Application.Interfaces.Repositories;
using Backend.Application.Interfaces.Services;
using Backend.Core.Errors;
using Backend.Core.Models;
using System.Data;

namespace Backend.Application.Services
{
    public class MoneyTransferService(
        IAccountRepository _accountRepository,
        ITransferHistoryRepository _transferHistoryRepository,
        ILogger<MoneyTransferService> _logger)
        : IMoneyTransferService
    {
        private readonly IAccountRepository _accountRepository = _accountRepository;
        private readonly ITransferHistoryRepository _transferHistoryRepository = _transferHistoryRepository;
        private readonly ILogger<MoneyTransferService> _logger = _logger;

        public async Task<List<DailyTransferSummaryDto>> GetTransferHistory(MoneyTransferHistoryRequest request)
        {
            var transfers = await _accountRepository.GetAccountsByUserIdWithTransferHistory(request);

            var outcomeTransfersDto = transfers
                .SelectMany(x => x.TransfersFrom)
                .GroupBy(x => x.TransferDate)
                .Select(x => new DailyTransferSummaryDto
                {
                    TransferedDate = x.Key,
                    SpentAmount = x.Sum(y => y.Amount)
                })
                .ToList();

            var incomeTransfersDto = transfers
                .SelectMany(x => x.TransfersTo)
                .GroupBy(x => x.TransferDate)
                .Select(x => new DailyTransferSummaryDto
                {
                    TransferedDate = x.Key,
                    ReceivedAmount = x.Sum(y => y.Amount)
                })
                .ToList();

            var result= outcomeTransfersDto
                .Union(incomeTransfersDto)
                .GroupBy(x => x.TransferedDate)
                .Select(x => new DailyTransferSummaryDto
                {
                    TransferedDate = x.Key,
                    SpentAmount = x.Sum(y => y.SpentAmount),
                    ReceivedAmount = x.Sum(y => y.ReceivedAmount)
                })
                .ToList();

            return result;
        }
        public async Task<Result> TransferToPerson(int accountId, int toAccountId, decimal money)
        {
            _logger.LogInformation(
                "Начало операции: {@OperationName}, {@DateTimeUtc}",
                nameof(TransferToPerson),
                DateTime.UtcNow
            );

            var fromAccount = await _accountRepository.GetAccountByIdAsync(accountId);
            var toAccount = await _accountRepository.GetAccountByIdAsync(toAccountId);

            if (fromAccount == null || toAccount == null)
            {
                _logger.LogInformation(
                   "Ошибка: {@OperationName}, {@Error}, {@DateTimeUtc}",
                   nameof(TransferToPerson),
                   MoneyTransferError.NotFound,
                   DateTime.UtcNow
                );

                return Result.Failure(MoneyTransferError.NotFound);
            }

            if (fromAccount.Balance < money)
            {
                _logger.LogInformation(
                  "Ошибка: {@OperationName}, {@Error}, {@DateTimeUtc}",
                  nameof(TransferToPerson),
                  MoneyTransferError.Balance,
                  DateTime.UtcNow
               );

                return Result.Failure(MoneyTransferError.Balance);
            }

            using var transaction = _accountRepository.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                await _accountRepository.ChangeBalance(fromAccount, -money);
                await _accountRepository.ChangeBalance(toAccount, money);

                await _transferHistoryRepository.Create(new Transfer
                {
                    Amount = money,
                    FromAccountId = fromAccount.Id,
                    ToAccountId = toAccount.Id,
                    TransferDate = DateTime.UtcNow
                });

                await transaction.CommitAsync();

                _logger.LogInformation(
                    "Выполнено успешно: {@OperationName}, {@DateTimeUtc}",
                    nameof(TransferToPerson),
                    DateTime.UtcNow
                );

                return Result.Success();
            }
            catch (Exception e)
            {
                _logger.LogInformation(
                   "Ошибка: {@OperationName}, {@Error}, {@DateTimeUtc}",
                   nameof(TransferToPerson),
                   e.Message,
                   DateTime.UtcNow
                );
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<MoneyTransferFilterResponse> GetFilters(int userId)
        {
            var accounts = await _accountRepository.GetAccountsByUserId(userId);

            var accountsOptions = accounts
                .Select(x => new Option { Label = x.Name, Value = x.Name })
                .ToList();

            var currencyOptions = accounts
                .Select(x => new Option { Label = x.FirstCurrency.Name, Value = x.FirstCurrency.Name })
                .ToList();

            currencyOptions.AddRange(accounts
                .Select(x => new Option { Label = x.SecondCurrency.Name, Value = x.SecondCurrency.Name })
                .ToList()
            );

            currencyOptions = currencyOptions.DistinctBy(x => x.Value).ToList();

            return new MoneyTransferFilterResponse { AccountOptions = accountsOptions, CurrencyOptions = currencyOptions };
        }
    }
}

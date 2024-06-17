using Backend.Application.Common;
using Backend.Application.Contracts.DTO;
using Backend.Application.Contracts.Request;
using Backend.Application.Interfaces.Repositories;
using Backend.Application.Interfaces.Services;
using Backend.Core.Errors;
using Backend.Core.Models;

namespace Backend.Application.Services
{
    public class AccountService(IAccountRepository _accountRepository, ICurrencyRepository _currencyRepository, ICoefficientRepository _coefficientRepository)
        : IAccountService
    {
        private readonly IAccountRepository _accountRepository = _accountRepository;
        private readonly ICurrencyRepository _currencyRepository = _currencyRepository;

        public async Task<Result<AccountDto>> CreateAsync(CreateAccountRequest request)
        {
            var accountsForUser = await _accountRepository.GetAccountsByUserId(request.userId);

            if(accountsForUser.Any(x => x.Name == request.name))
            {
                return Result.Failure<AccountDto>(AccountError.UserAlreadyExist);
            }

            var newAccount = new Account
            {
                UserId = request.userId,
                Name = request.name,
                Balance = request.balance,
                FirstCurrencyId = request.firstCurrencyId,
                SecondCurrencyId = request.secondCurrencyId,
                MainCurrencyId = request.firstCurrencyId
            };

            var firstCurrency = await _currencyRepository.GetByIdAsync(request.firstCurrencyId);
            var secondCurrency = await _currencyRepository.GetByIdAsync(request.secondCurrencyId);

            if(firstCurrency == null || secondCurrency == null)
            {
                return Result.Failure<AccountDto>(AccountError.NotFound);
            }

            await _accountRepository.Create(newAccount);

            return Result.Success<AccountDto>(new AccountDto
            {
                Id = newAccount.Id,
                Name = newAccount.Name,
                Balance = newAccount.Balance,
                FirstCurrencyName = firstCurrency.Name,
                SecondCurrencyName = secondCurrency.Name,
                IsFirstCurrencyMain = true
            });
        }

        public async Task<Result<ConvertDto>> Convert(int accountId)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);

            if(account is null)
            {
                return Result.Failure<ConvertDto>(AccountError.NotFound);
            }

            var coeff = 0d;

            if(account.MainCurrencyId == account.FirstCurrencyId)
            {
                var converter = await _coefficientRepository.GetByFromAndToIds(account.FirstCurrencyId, account.SecondCurrencyId);

                if(converter is null)
                {
                    return Result.Failure<ConvertDto>(CurrencyConverterError.NotCreated);
                }

                coeff = converter.Coefficient;

                account.MainCurrencyId = account.SecondCurrencyId;
            }
            else
            {
                var converter = await _coefficientRepository.GetByFromAndToIds(account.SecondCurrencyId, account.FirstCurrencyId);

                if(converter is null)
                {
                    return Result.Failure<ConvertDto>(CurrencyConverterError.NotCreated);
                }

                coeff = converter.Coefficient;

                account.MainCurrencyId = account.FirstCurrencyId;
            }

            account.Balance = Math.Round(account.Balance * (decimal)coeff, 2);
            await _accountRepository.Create(account);

            return Result.Success<ConvertDto>(new ConvertDto
            {
                Balance = account.Balance,
                IsFirstMain = account.MainCurrencyId == account.FirstCurrencyId
            });
        }

        public async Task<List<AccountDto>> GetByUserId(int userId)
        {
            var accounts = await _accountRepository.GetAccountsByUserId(userId);

            return accounts
                .Select(x => new AccountDto
                {
                    Id = x.Id,
                    Balance = x.Balance,
                    Name = x.Name,
                    FirstCurrencyName = x.FirstCurrency.Name,
                    SecondCurrencyName = x.SecondCurrency.Name,
                    IsFirstCurrencyMain = x.MainCurrencyId == x.FirstCurrencyId
                })
                .ToList();
        }

        public async Task<(List<AccountDto>, List<TransferAccountDto>)> GetAccountsForTransfer(int currentUserId)
        {
            var allAccounts = await _accountRepository.GetAllAccountsAsync();
            var currentUserAccounts = await _accountRepository.GetAccountsByUserId(currentUserId);

            var allAccountDtos = allAccounts
                .Select(x => new TransferAccountDto { Id = x.Id, Name = x.Name, UserName = x.User.UserName })
                .ToList();

            var currentUserAccountsDots = currentUserAccounts
               .Select(x => new AccountDto{ Id = x.Id, Name = x.Name, Balance = x.Balance })
               .ToList();

            return (currentUserAccountsDots, allAccountDtos);
        }
    }
}

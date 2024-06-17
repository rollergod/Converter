using Backend.Application.Common;
using Backend.Application.Contracts.DTO;
using Backend.Application.Interfaces;
using Backend.Application.Interfaces.Repositories;
using Backend.Application.Interfaces.Services;
using Backend.Core.Errors;

namespace Backend.Application.Services
{
    public class UserService(
        IJwtGenerator _jwtGenerator,
        IUserRepository _userRepository,
        IPasswordHasher _passwordHasher,
        IAccountRepository _accountRepository,
        ILogger<UserService> _logger)
        : IUserService
    {
        private readonly IJwtGenerator _jwtGenerator = _jwtGenerator;
        private readonly IUserRepository _userRepository = _userRepository;
        private readonly IPasswordHasher _passwordHasher = _passwordHasher;
        private readonly IAccountRepository _accountRepository = _accountRepository;
        private readonly ILogger<UserService> _logger = _logger;

        public async Task<Result> Register(string userName, string password)
        {
            _logger.LogInformation(
                "Начало операции: {@OperationName}, {@DateTimeUtc}",
                nameof(Register),
                DateTime.UtcNow
            );

            // проверка на имя
            var isUserWithCurrentNameExist = await _userRepository.GetByUserName(userName);

            if (isUserWithCurrentNameExist != null)
            {
                _logger.LogInformation(
                    "Ошибка: {@OperationName}, {@Error},{@DateTimeUtc}",
                    nameof(Register),
                    UserError.Exist,
                    DateTime.UtcNow
                );
                return Result.Failure(UserError.Exist);
            }

            var hashed = _passwordHasher.Generate(password);
            await _userRepository.Create(userName, hashed);

            _logger.LogInformation(
                "Выполнено успешно: {@OperationName}, {@DateTimeUtc}",
                nameof(Register),
                DateTime.UtcNow
            );
            return Result.Success();
        }

        public async Task<Result<UserDto>> Login(string userName, string password)
        {
            var user = await _userRepository.GetByUserName(userName);

            if(user is null)
            {
                return Result.Failure<UserDto>(UserError.NotFound);
            }

            var result = _passwordHasher.Verify(password, user.HashedPassword);

            if(result == false)
            {
                return Result.Failure<UserDto>(UserError.BadCredentials);
            }

            var userAccounts = await _accountRepository.GetAccountsByUserId(user.Id);

            var token = _jwtGenerator.Generate(user);

            var dto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Accounts = userAccounts.Select(x => new AccountDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Balance = x.Balance,
                    FirstCurrencyName = x.FirstCurrency.Name,
                    SecondCurrencyName = x.SecondCurrency.Name,
                    IsFirstCurrencyMain = x.MainCurrencyId == x.FirstCurrencyId
                }).ToList(),
                Token = token
            };

            return Result.Success<UserDto>(dto);
        }
    }
}

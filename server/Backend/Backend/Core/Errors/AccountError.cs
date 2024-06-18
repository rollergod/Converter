using Backend.Application.Common;

namespace Backend.Core.Errors
{
    public static class AccountError
    {
        public static readonly Error UserAlreadyExist = Error.Validation("AccountError","Название для счета уже используется");
        public static readonly Error CantCreateMore5Accounts = Error.Validation("AccountError", "Максимальное количество для одного пользователя - 5 аккаунтов");
        public static readonly Error NotFound = Error.NotFound("AccountError", "Счет не найден");
    }
}

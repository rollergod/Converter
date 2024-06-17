using Backend.Application.Common;

namespace Backend.Core.Errors
{
    public static class UserError
    {
        public static readonly Error NotFound = Error.NotFound("UserService", "Пользователь не найден");
        public static readonly Error Exist = Error.Validation("UserService", "Пользователь с таким логином уже существует");
        public static readonly Error BadCredentials = Error.NotFound("UserService", "Введены неверные данные для входа");
    }
}

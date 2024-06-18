using Backend.Application.Common;

namespace Backend.Core.Errors
{
    public static class AuthError
    {
        public static readonly Error BadAuth = Error.Auth("AuthError", "Неудачная авторизация. Войдите заного");
        public static readonly Error ClaimUserIdNotFound = Error.Auth("ClaimError", "Не удалось найти данные пользователя по токену");
    }
}

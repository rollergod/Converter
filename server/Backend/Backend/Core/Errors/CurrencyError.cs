using Backend.Application.Common;

namespace Backend.Core.Errors
{
    public static class CurrencyError
    {
        public static readonly Error NotFound = Error.NotFound("Currency", "Не удалось найти валюту");

        public static readonly Error Exist = Error.Validation("Currency", "Валюта с таким наименованием уже существует");
    }
}

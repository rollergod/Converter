using Backend.Application.Common;

namespace Backend.Core.Errors
{
    public static class CurrencyConverterError
    {
        public readonly static Error FromAndToIdsEquals = Error.Validation(
            "CurrencyConverter",
            "Указана одна и та же валюта"
        );

        public readonly static Error Exist = Error.Validation(
            "CurrencyConveter",
            "Перевод для выбранной валюты уже создан"
        );

        public readonly static Error NotFound = Error.NotFound(
           "CurrencyConveter",
           "Перевод не найден"
        );

        public readonly static Error NotCreated = Error.NotFound(
           "CurrencyConveter",
           "Не указан коэффициент для перевода"
        );
    }
}

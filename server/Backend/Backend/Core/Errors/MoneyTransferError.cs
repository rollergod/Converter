using Backend.Application.Common;

namespace Backend.Core.Errors
{
    public static class MoneyTransferError
    {
        public static readonly Error NotFound = Error.NotFound("MoneyTransferService", "Счет для перевода не найден");
        public static readonly Error Balance = Error.Validation("MoneyTransferService", "На счете недостаточно средств");
    }
}

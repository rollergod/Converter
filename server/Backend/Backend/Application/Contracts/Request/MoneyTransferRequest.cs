namespace Backend.Application.Contracts.Request
{
    public record MoneyTransferRequest(int fromAccountId, int toAccountId, decimal money);
}

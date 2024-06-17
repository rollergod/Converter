namespace Backend.Application.Contracts.Request
{
    public record CreateAccountRequest(int userId, string name, decimal balance, int firstCurrencyId, int secondCurrencyId);
}

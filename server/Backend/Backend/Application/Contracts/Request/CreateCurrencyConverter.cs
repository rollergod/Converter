namespace Backend.Application.Contracts.Request
{
    public record CreateCurrencyConverter(int fromId, int toId, double coefficient);
}

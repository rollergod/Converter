using Backend.Application.Common;

namespace Backend.Application.Contracts.Response
{
    public class MoneyTransferFilterResponse
    {
        public List<Option> CurrencyOptions { get; set; }
        public List<Option> AccountOptions { get; set; }
    }
}

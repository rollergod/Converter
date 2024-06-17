using System.Diagnostics.CodeAnalysis;

namespace Backend.Application.Contracts.Request
{
    public class MoneyTransferHistoryRequest : IParsable<MoneyTransferHistoryRequest>
    {
        public int UserId { get; set; }
        public List<string> CurrencyIds { get; set; }
        public List<string> AccountIds { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public static MoneyTransferHistoryRequest Parse(string s, IFormatProvider? provider)
        {
            var result = new MoneyTransferHistoryRequest();
            TryParse(s, provider, out result);
            return result;
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out MoneyTransferHistoryRequest result)
        {
            result = null;

            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            var keyValuePairs = s.Split('&');

            result = new MoneyTransferHistoryRequest();

            foreach (var kvp in keyValuePairs)
            {
                var keyValue = kvp.Split('=');
                if (keyValue.Length != 2)
                {
                    continue;
                }

                var key = keyValue[0];
                var value = keyValue[1];

                switch (key)
                {
                    case "userId":
                        if (int.TryParse(value, out int userId))
                        {
                            result.UserId = userId;
                        }
                        break;
                    case "startDate":
                        if (DateTime.TryParse(value, out DateTime startDate))
                        {
                            result.StartDate = startDate;
                        }
                        break;
                    case "endDate":
                        if (DateTime.TryParse(value, out DateTime endDate))
                        {
                            result.EndDate = endDate;
                        }
                        break;
                    case "accountIds":
                        result.AccountIds = value.Split(',').ToList();
                        break;
                    case "currencyIds":
                        result.CurrencyIds = value.Split(',').ToList();
                        break;
                }
            }

            return true;
        }
    }
}

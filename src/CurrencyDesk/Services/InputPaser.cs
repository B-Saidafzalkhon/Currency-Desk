using System.Globalization;
namespace CurrencyDesk.Services
{
    public class InputParser
    {
        public static bool TryParseAmount(string? input, out decimal amount)
        {
            string? normalizedInput = input?.Replace(",", ".").Trim();

            if (decimal.TryParse(normalizedInput, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out amount) && amount > 0)
                return true;

            amount = 0;
            return false;
        }
        public static bool TryParseCurrencyCode(string? input, out string code)
        {
            string cleaned = (input ?? "").Replace(" ", "").ToUpperInvariant();
            if(cleaned.Length == 3 && cleaned.All(char.IsAsciiLetter))
            {
                code = cleaned;
                return true;
            }
            code = string.Empty;
            return false;
        }
    }
}

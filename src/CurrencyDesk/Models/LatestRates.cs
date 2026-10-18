using System.Text;
using System.Text.Json.Serialization;

namespace CurrencyDesk.Models
{
    public class LatestRates
    {
        [JsonPropertyName("amount")]
        public decimal Amount { get; init; }
        [JsonPropertyName("base")]
        public string Base { get; init; } = "";
        [JsonPropertyName("date")]
        public string Date { get; init; } = "";
        [JsonPropertyName("rates")]
        public Dictionary<string, decimal> Rates { get; init; } = new();

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var item in Rates)
                sb.AppendLine($" {item.Key}: {item.Value}");
            
            return $"Amount: {Amount}\nBase: {Base}\nDate: {Date}\nRates:\n{sb.ToString()}";
        }
    }
}

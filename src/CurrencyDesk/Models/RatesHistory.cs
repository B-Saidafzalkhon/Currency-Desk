using System.Text;
using System.Text.Json.Serialization;

namespace CurrencyDesk.Models
{
    public class RatesHistory
    {
        [JsonPropertyName("base")]
        public string Base { get; init; } = "";
        [JsonPropertyName("start_date")]
        public string StartDate { get; init; } = "";
        [JsonPropertyName("end_date")]
        public string EndDate { get; init; } = "";
        [JsonPropertyName("rates")]
        public Dictionary<string, Dictionary<string, decimal>> Rates { get; init; } = new();

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var item in Rates)
            {
                sb.Append($"{item.Key}: ");
                foreach(var pair in item.Value)
                {
                    sb.AppendLine($" {pair.Key}: {pair.Value}");
                }
            }
           
            return $"Base: {Base}\nStart Date: {StartDate}\nEnd Date: {EndDate}\nRates:\n{sb.ToString()}";
        }
    }
}

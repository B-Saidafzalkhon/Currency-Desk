using CurrencyDesk.Models;
using System.Net;
using System.Net.Http.Json;
using System.Globalization;

namespace CurrencyDesk.Clients
{
    public class FrankfurterClient
    {
        private readonly HttpClient _http;
        public FrankfurterClient(HttpClient http)
        {
            ArgumentNullException.ThrowIfNull(http);
            _http = http;
        }

        private static void ValidateCurrencyCode(string code, string paramName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(code, paramName);
            if (code.Length != 3)
                throw new ArgumentException("Currency code must have 3 letters.", paramName);
        }
        public async Task<LatestRates> GetLatestRatesAsync(string baseCurrency, CancellationToken ct)
        {
            ValidateCurrencyCode(baseCurrency, nameof(baseCurrency));

            using var response = await _http.GetAsync($"latest?from={baseCurrency}", ct);

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new ArgumentException($"Currency '{baseCurrency}' not found.", nameof(baseCurrency));

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<LatestRates>(ct)
                ?? throw new InvalidOperationException("Empty response from API");
        }

        public async Task<IReadOnlyDictionary<string, string>> GetCurrenciesAsync(CancellationToken ct)
        {
            using var response = await _http.GetAsync("currencies", ct);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IReadOnlyDictionary<string, string>>(ct)
                ?? throw new InvalidOperationException("Empty response from API");
        }
        public async Task<RatesHistory> GetHistoryAsync(string baseCurrency, string target, DateOnly from, DateOnly to, CancellationToken ct)
        {
            ValidateCurrencyCode(baseCurrency, nameof(baseCurrency));

            ValidateCurrencyCode(target, nameof(target));

            if (baseCurrency == target)
                throw new ArgumentException("Target and base currency cannot be equal.", nameof(target));

            if (from > to)
                throw new ArgumentException("Start date must not be after end date.", nameof(from));

            string start = from.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string end = to.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            using var response = await _http.GetAsync($"{start}..{end}?from={baseCurrency}&to={target}", ct);

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new ArgumentException($"No rates for {baseCurrency} -> {target} in {start}..{end}.");

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<RatesHistory>(ct)
                ?? throw new InvalidOperationException("Empty response from API");
        }
    }
}

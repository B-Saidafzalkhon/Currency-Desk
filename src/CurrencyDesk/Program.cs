using CurrencyDesk.Clients;

namespace CurrencyDesk;

internal class Program
{
    static async Task Main(string[] args)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        var http = new HttpClient()
        {
            BaseAddress = new Uri("https://api.frankfurter.dev/v1/"),
            Timeout = TimeSpan.FromSeconds(30)
        };
        var api = new FrankfurterClient(http);

        var ratesHistory = await api.GetHistoryAsync("USD", "EUR", new DateOnly(2026, 09, 01), new DateOnly(2026, 09, 30), cts.Token);
        var latestRates = await api.GetLatestRatesAsync("USD", cts.Token);

        Console.WriteLine(latestRates);
        Console.WriteLine(ratesHistory);
    }
}
using CurrencyDesk.Clients;
using CurrencyDesk.UI;

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

        var client = new FrankfurterClient(http);
        var menu = new ConsoleMenu(client);

        await menu.RunAsync();
    }
}
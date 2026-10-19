using System.Text.Json;
using CurrencyDesk.Clients;
using CurrencyDesk.Services;

namespace CurrencyDesk.UI;

public class ConsoleMenu
{
    private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(10);
    private readonly FrankfurterClient _client;

    public ConsoleMenu(FrankfurterClient client)
    {
        ArgumentNullException.ThrowIfNull(client);
        _client = client;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== CurrencyDesk ===");
            Console.WriteLine("1. Latest rates");
            Console.WriteLine("2. Convert");
            Console.WriteLine("0. Exit");
            Console.Write("> ");

            string? choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    Console.Clear();
                    await RunSafelyAsync(ShowRatesAsync);
                    Console.ReadKey();
                    break;
                case "2":
                    Console.Clear();
                    await RunSafelyAsync(ConvertAsync);
                    Console.ReadKey();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Unknown option.");
                    Console.ReadKey();
                    break;
            }
        }
    }
    private static async Task RunSafelyAsync(Func<Task> operation)
    {
        try
        {
            await operation();
        }
        catch (HttpRequestException)
        {
            Console.WriteLine("No connection to the rates server. Check your internet and try again.");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("The server did not respond in time. Try again later.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (JsonException)
        {
            Console.WriteLine("The server returned data in an unexpected format.");
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("The server returned an empty response.");
        }
    }

    private async Task ShowRatesAsync()
    {
        string baseCurrency = AskCurrency("Base currency (e.g. USD): ");

        using var cts = new CancellationTokenSource(RequestTimeout);   
        var latest = await _client.GetLatestRatesAsync(baseCurrency, cts.Token);

        Console.WriteLine($"Rates for 1 {latest.Base} on {latest.Date}:");
        foreach (var (code, rate) in latest.Rates.OrderBy(r => r.Key))
            Console.WriteLine($"  {code}: {rate:F4}");
    }

    private async Task ConvertAsync()
    {
        decimal amount = AskAmount("Amount: ");
        string from = AskCurrency("From currency: ");
        string to = AskCurrency("To currency: ");

        if (from == to)                   
        {
            Console.WriteLine($"{amount:N2} {from} = {amount:N2} {to}");
            return;
        }

        using var cts = new CancellationTokenSource(RequestTimeout);
        var latest = await _client.GetLatestRatesAsync(from, cts.Token);

        if (!latest.Rates.TryGetValue(to, out decimal rate))
        {
            Console.WriteLine($"Rate {from} -> {to} is not available.");
            return;
        }

        decimal result = CurrencyConverter.Convert(amount, rate);
        Console.WriteLine($"{amount:N2} {from} = {result:N2} {to}  (rate {rate}, {latest.Date})");
    }

    private static string AskCurrency(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (InputParser.TryParseCurrencyCode(Console.ReadLine(), out string code))
                return code;
            Console.WriteLine("Enter a 3-letter code, for example USD.");
        }
    }
    private static decimal AskAmount(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (InputParser.TryParseAmount(Console.ReadLine(), out decimal amount))
                return amount;
            Console.WriteLine("Enter a positive number, for example 100 or 100.50.");
        }
    }
}
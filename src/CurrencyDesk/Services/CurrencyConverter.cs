namespace CurrencyDesk.Services
{
    public class CurrencyConverter
    {
        public static decimal Convert(decimal amount, decimal rate)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Sum cannot be < 0");

            if (rate <= 0)
                throw new ArgumentOutOfRangeException(nameof(rate), "Rate should be > 0");

            decimal result = amount * rate;
            return Math.Round(result, 2,MidpointRounding.AwayFromZero);
        }
    }
}

using CurrencyDesk.Services;
using System.Globalization;

namespace CurrencyDesk.Tests.Services
{
    public class InputParserTests
    {
        [Theory]
        [InlineData("100", "100")]
        [InlineData("100.5","100.5")]
        [InlineData("100,5", "100.5")]
        [InlineData("  42 " , "42")]
        public void TryParseAmount_CheckInput_ReturnsTrue(string? input, string? expected)
        {
            bool check = InputParser.TryParseAmount(input, out decimal amount);

            Assert.True(check);
            Assert.Equal(decimal.Parse(expected, CultureInfo.InvariantCulture), amount);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("-5")]
        [InlineData("abc")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("1,5,0")]
        public void TryParseAmount_CheckInput_ReturnsFalse(string? amount)
        {
            bool check = InputParser.TryParseAmount(amount, out decimal result);

            Assert.False(check);
        }

        [Theory]
        [InlineData("usd")]
        [InlineData(" eur ")]
        public void TryParseCurrencyCode_CheckCurrencyCode_ReturnTrue(string? code)
        {
            bool check = InputParser.TryParseCurrencyCode(code, out string result);

            Assert.True(check);
        }
        [Theory]
        [InlineData("US")]
        [InlineData("USDT")]
        public void TryParseCurrencyCode_CheckCurrencyCode_ReturnFalse(string? code)
        {
            bool check = InputParser.TryParseCurrencyCode(code, out string result);

            Assert.False(check);
        }
    }
}

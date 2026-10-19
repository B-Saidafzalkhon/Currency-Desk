using CurrencyDesk.Services;

namespace CurrencyDesk.Tests.Services
{
    public class CurrencyConverterTests
    {
        [Fact]
        public void Convert_CheckAmountAndRate_ReturnsRoundedResult()
        {
            decimal amount = 100m;
            decimal rate = 0.88865m;
            decimal expected = 88.87m;

            decimal result = CurrencyConverter.Convert(amount, rate);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Convert_CheckAmountIsZero_ReturnsZero()
        {
            decimal amount = 0m;
            decimal rate = 1.5m;
            decimal expected = 0m;

            decimal result = CurrencyConverter.Convert(amount, rate);

            Assert.Equal(expected, amount);
        }

        [Fact]
        public void Convert_CheckAmountIsNegative_ThrowsArgumentOutOfRangeException()
        {
            decimal negativeAmount = -10m;
            decimal validRate = 1.5m;

            var ex = Assert.Throws<ArgumentOutOfRangeException>
                (() =>
                CurrencyConverter.Convert(negativeAmount, validRate)
                );

            Assert.Equal("amount", ex.ParamName);
        }

        [Fact]
        public void Convert_CheckRateIsZero_ThrowsArgumentOutOfRangeException()
        {
            decimal validAmount = 10m;
            decimal zeroRate = 0;

            var ex = Assert.Throws<ArgumentOutOfRangeException>
                (() =>
                CurrencyConverter.Convert(validAmount, zeroRate)
                );

            Assert.Equal("rate", ex.ParamName);
        }

        [Fact]
        public void Convert_CheckRateIsNegative_ThrowsArgumentOutOfRangeException()
        {
            decimal validAmount = 10m;
            decimal negativeRate = -1.5m;

            var ex = Assert.Throws<ArgumentOutOfRangeException>
                (() =>
                CurrencyConverter.Convert(validAmount, negativeRate)
                );

            Assert.Equal("rate", ex.ParamName);
        }
    }
}

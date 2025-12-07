using PayTR.PosSelection.Domain.Aggregates;
using PayTR.PosSelection.Domain.ValueObjects;
using Xunit;

namespace PayTR.PosSelection.Tests;

public class CostCalculationTests
{
    [Theory]
    [InlineData(362.22, 0.026, 0, "TRY", 9.42)]
    [InlineData(60.00, 0.0229, 0, "TRY", 1.37)]
    [InlineData(100.00, 0.031, 0, "TRY", 3.10)]
    public void TryCostCalculation(decimal amount, decimal rate, decimal minFee, string curr, decimal expected)
    {
        var currency = Currency.FromCode(curr);
        var pos = CreatePos(currency, rate, minFee);
        
        var result = pos.CalculateCost(new Money(amount, currency));
        
        Assert.Equal(expected, result.Amount);
    }

    [Fact]
    public void UsdHasMultiplier()
    {
        var pos = CreatePos(Currency.USD, 0.031m, 0);
        var result = pos.CalculateCost(new Money(395m, Currency.USD));
        Assert.Equal(12.37m, result.Amount);
    }

    [Fact]
    public void MinFeeApplied()
    {
        var pos = CreatePos(Currency.TRY, 0.02m, 2m);
        var result = pos.CalculateCost(new Money(50m, Currency.TRY));
        Assert.Equal(2m, result.Amount);
    }

    static PosRate CreatePos(Currency curr, decimal rate, decimal minFee)
    {
        return new PosRate(
            "TestPOS",
            new CardInfo("credit", "bonus"),
            6, curr,
            new CommissionRate(rate),
            new Money(minFee, curr),
            1);
    }
}


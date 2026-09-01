using Dfe.Acec.RulesEngine.Helpers;

namespace Dfe.Acec.RulesEngine.Tests.Unit.Helpers;

public class TermDateCalculatorTests
{
    [Fact]
    public void GetNextTermStartDateWhenDateIsInSpringTermReturnsAprilFirst()
    {
        var date = new DateOnly(2025, 2, 15);

        var result = TermDateCalculator.GetNextTermStartDate(date);

        Assert.Equal(
            new DateOnly(2025, 4, 1),
            result);
    }

    [Fact]
    public void GetNextTermStartDateWhenDateIsInSummerTermReturnsSeptemberFirst()
    {
        var date = new DateOnly(2025, 6, 15);

        var result = TermDateCalculator.GetNextTermStartDate(date);

        Assert.Equal(
            new DateOnly(2025, 9, 1),
            result);
    }

    [Fact]
    public void GetNextTermStartDateWhenDateIsInAutumnTermReturnsJanuaryFirstNextYear()
    {
        var date = new DateOnly(2025, 10, 15);

        var result = TermDateCalculator.GetNextTermStartDate(date);

        Assert.Equal(
            new DateOnly(2026, 1, 1),
            result);
    }
}

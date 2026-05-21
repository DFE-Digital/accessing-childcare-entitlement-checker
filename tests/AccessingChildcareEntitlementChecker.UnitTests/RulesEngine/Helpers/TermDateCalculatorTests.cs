using AccessingChildcareEntitlementChecker.RulesEngine.Helpers;

namespace AccessingChildcareEntitlementChecker.UnitTests.RulesEngine.Helpers;

public class TermDateCalculatorTests
{
    [Fact]
    public void GetNextTermStartDate_WhenDateIsInSpringTerm_ReturnsAprilFirst()
    {
        var date = new DateOnly(2025, 2, 15);

        var result = TermDateCalculator.GetNextTermStartDate(date);

        Assert.Equal(
            new DateOnly(2025, 4, 1),
            result);
    }

    [Fact]
    public void GetNextTermStartDate_WhenDateIsInSummerTerm_ReturnsSeptemberFirst()
    {
        var date = new DateOnly(2025, 6, 15);

        var result = TermDateCalculator.GetNextTermStartDate(date);

        Assert.Equal(
            new DateOnly(2025, 9, 1),
            result);
    }

    [Fact]
    public void GetNextTermStartDate_WhenDateIsInAutumnTerm_ReturnsJanuaryFirstNextYear()
    {
        var date = new DateOnly(2025, 10, 15);

        var result = TermDateCalculator.GetNextTermStartDate(date);

        Assert.Equal(
            new DateOnly(2026, 1, 1),
            result);
    }
}
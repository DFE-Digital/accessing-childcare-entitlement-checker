using Dfe.Acec.Web.Models;
using System.Diagnostics;

namespace Dfe.Acec.Tests.Unit.Models;

public class WeeklyEarningsThresholdsTests
{
    [Theory]
    [InlineData(AgeRange.UnderEighteen, "128")]
    [InlineData(AgeRange.EighteenToTwenty, "173")]
    [InlineData(AgeRange.TwentyOneOrOver, "203")]
    public void ReturnsExpectedWeeklyThreshold(AgeRange ageRange, string expected)
    {
        var actual = WeeklyEarningsThresholds.Create(ageRange, [WorkStatusOption.PaidEmployment]);
        Assert.Equal(expected, actual.PerWeek);
    }

    [Fact]
    public void ThrowsIfAgeRangeIsNotAnswered()
    {
        Assert.Throws<InvalidOperationException>(() => WeeklyEarningsThresholds.Create(null, []));
    }

    [Fact]
    public void ThrowsIfWorkStatusIsNotAnswered()
    {
        Assert.Throws<InvalidOperationException>(() => WeeklyEarningsThresholds.Create(AgeRange.EighteenToTwenty, []));
    }

    [Fact]
    public void CoverageThrowsIfInvalidAgeRangePassed()
    {
        var invalid = (AgeRange)99;
        Assert.Throws<UnreachableException>(() => WeeklyEarningsThresholds.Create(invalid, [WorkStatusOption.PaidEmployment]));
    }
}

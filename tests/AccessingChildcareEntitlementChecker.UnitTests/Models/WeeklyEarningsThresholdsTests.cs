using AccessingChildcareEntitlementChecker.Web.Models;
using System.Diagnostics;

namespace AccessingChildcareEntitlementChecker.UnitTests.Models;

public class WeeklyEarningsThresholdsTests
{
    [Theory]
    [InlineData(AgeRange.UnderEighteen, "128")]
    [InlineData(AgeRange.EighteenToTwenty, "173")]
    [InlineData(AgeRange.TwentyOneOrOver, "203")]
    public void Returns_Expected_Weekly_Threshold(AgeRange ageRange, string expected)
    {
        var actual = WeeklyEarningsThresholds.Create(ageRange, [WorkStatusOption.PaidEmployment]);
        Assert.Equal(expected, actual.PerWeek);
    }

    [Fact]
    public void Throws_If_Age_Range_Is_Not_Answered()
    {
        Assert.Throws<InvalidOperationException>(() => WeeklyEarningsThresholds.Create(null, []));
    }

    [Fact]
    public void Throws_If_Work_Status_Is_Not_Answered()
    {
        Assert.Throws<InvalidOperationException>(() => WeeklyEarningsThresholds.Create(AgeRange.EighteenToTwenty, []));
    }

    [Fact]
    public void Coverage_Throws_If_Invalid_AgeRange_Passed()
    {
        var invalid = (AgeRange)99;
        Assert.Throws<UnreachableException>(() => WeeklyEarningsThresholds.Create(invalid, [WorkStatusOption.PaidEmployment]));
    }
}

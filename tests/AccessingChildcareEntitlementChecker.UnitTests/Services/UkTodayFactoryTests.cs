using AccessingChildcareEntitlementChecker.Web.Services;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Services;

public class UkTodayFactoryTests
{
    [Fact]
    public void Today_ReturnsUkDate_WhenUtcDateIsStillPreviousDay()
    {
        var dateTimeFactory = Substitute.For<IDateTimeFactory>();
        dateTimeFactory.UtcNow.Returns(new DateTime(2026, 5, 11, 23, 30, 0, DateTimeKind.Utc));
        var factory = new UkTodayFactory(dateTimeFactory);
        var today = factory.Today;
        Assert.Equal(new DateOnly(2026, 5, 12), today);
    }

    [Fact]
    public void Today_ReturnsSameDate_WhenUkAndUtcAreOnSameDay()
    {
        var dateTimeFactory = Substitute.For<IDateTimeFactory>();
        dateTimeFactory.UtcNow.Returns(new DateTime(2026, 5, 11, 12, 0, 0, DateTimeKind.Utc));
        var factory = new UkTodayFactory(dateTimeFactory);
        var today = factory.Today;
        Assert.Equal(new DateOnly(2026, 5, 11), today);
    }
}

using Microsoft.Extensions.Localization;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests;

public static class AcecSubstitute
{
    public static IStringLocalizerFactory ForLocalizerFactory()
    {
        var localizerFactory = Substitute.For<IStringLocalizerFactory>();
        var localizer = Substitute.For<IStringLocalizer>();
        localizer[Arg.Any<string>()].Returns(callInfo =>
        {
            var key = callInfo.Arg<string>();
            return new LocalizedString(key, key);
        });

        localizer[Arg.Any<string>(), Arg.Any<object[]>()].Returns(callInfo =>
        {
            var key = callInfo.Arg<string>();
            var args = callInfo.Arg<object[]>();
            var formattedValue = string.Format(key, args);
            return new LocalizedString(key, formattedValue);
        });

        localizerFactory.Create(Arg.Any<Type>()).Returns(localizer);
        localizerFactory.Create(Arg.Any<string>(), Arg.Any<string>()).Returns(localizer);

        return localizerFactory;
    }
}

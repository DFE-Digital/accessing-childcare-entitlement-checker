using System.ComponentModel.DataAnnotations;
using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.Partner;
using Dfe.Acec.Web.Services;
using Microsoft.Extensions.Localization;

namespace Dfe.Acec.Web.Tests.Unit.Models.Partner;

public class PartnerWeeklyEarningsViewModelTests
{
    private readonly JourneyState _journeyState;
    private readonly IStringLocalizerFactory _localizerFactory;
    private readonly Func<Type, object> _serviceProviderFunc;

    public PartnerWeeklyEarningsViewModelTests()
    {
        _journeyState = new JourneyState();
        _localizerFactory = AcecSubstitute.ForLocalizerFactory();
        _serviceProviderFunc = serviceType =>
        {
            if (serviceType == typeof(JourneyState))
            {
                return _journeyState;
            }

            if (serviceType == typeof(IStringLocalizerFactory))
            {
                return _localizerFactory;
            }

            return null!;
        };
    }

    [Fact]
    public void ValidateReturnsErrorWhenNoneSelected()
    {
        _journeyState.PartnerAge = AgeRange.UnderEighteen;
        _journeyState.PartnerWorkStatus = [WorkStatusOption.Apprentice];

        var model = new PartnerWeeklyEarningsViewModel()
        {
            PartnerWeeklyEarnings = null,
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("Select if your partner earns £128 a week or more before tax", validationResults[0].ErrorMessage);
    }

    [Fact]
    public void ValidateCoverageThrowsIfNoJourneyState()
    {
        var model = new PartnerWeeklyEarningsViewModel()
        {
            PartnerWeeklyEarnings = null,
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(ServiceProviderFunc);

        Assert.Throws<InvalidOperationException>(() => model.Validate(validationContext).ToList());
        return;

        object ServiceProviderFunc(Type serviceType)
        {
            return serviceType == typeof(IStringLocalizerFactory)
                ? _localizerFactory
                : null!;
        }
    }
}

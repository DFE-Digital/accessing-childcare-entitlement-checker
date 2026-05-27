using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class PartnerControllerTests
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly PartnerController _controller;

    public PartnerControllerTests()
    {
        _journeyState = new JourneyState();
        _journeySession = Substitute.For<IJourneySession>();
        _controller = new PartnerController(_journeyState, _journeySession);
    }

    [Fact]
    public void PartnerAge_ReturnsView()
    {
        var result = _controller.PartnerAge();
        Assert.Null(result.Model<PartnerAgeViewModel>().PartnerAge);
    }

    [Fact]
    public void PartnerAge_Get_PopulatesModel_FromState()
    {
        _journeyState.PartnerAge = AgeRange.EighteenToTwenty;
        var result = _controller.PartnerAge();
        Assert.Equal(AgeRange.EighteenToTwenty, result.Model<PartnerAgeViewModel>().PartnerAge);
    }

    [Fact]
    public void PartnerAge_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new PartnerAgeViewModel()
        {
            PartnerAge = AgeRange.EighteenToTwenty
        };

        var result = _controller.PartnerAge(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(AgeRange.EighteenToTwenty, _journeyState.PartnerAge);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(PartnerController.PartnerNationality), redirect.ActionName);
    }

    [Fact]
    public void PartnerAge_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new PartnerAgeViewModel
        {
            PartnerAge = null
        };

        _controller.ModelState.AddModelError(nameof(model.PartnerAge), "Faked Model Binding Error");

        var result = _controller.PartnerAge(model);

        var view = Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PartnerAge)));
    }

    [Theory]
    [InlineData(NationalityOption.BritishOrIrishCitizen, "Partner", nameof(PartnerController.PartnerPaidWork))]
    [InlineData(NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, "Partner", nameof(PartnerController.PartnerSettledStatus))]
    [InlineData(NationalityOption.CitizenOfADifferentCountry, "Partner", nameof(PartnerController.PartnerPaidWork))]
    public void PartnerNationality_Post_SavesState_AndRedirects(NationalityOption option, string controllerName, string actionName)
    {
        var model = new PartnerNationalityViewModel
        {
            PartnerNationality = option
        };
        var result = _controller.PartnerNationality(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(option, _journeyState.PartnerNationality);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal(controllerName, redirect.ControllerName);
    }

    [Fact]
    public void PartnerNationality_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new PartnerNationalityViewModel();
        _controller.ModelState.AddModelError(nameof(model.PartnerNationality), "Faked Model Binding Error");
        var result = _controller.PartnerNationality(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PartnerNationality)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void PartnerNationality_Get_PopulatesModel_FromState()
    {
        _journeyState.PartnerNationality = NationalityOption.BritishOrIrishCitizen;
        var result = Assert.IsType<ViewResult>(_controller.PartnerNationality());
        Assert.Equal(NationalityOption.BritishOrIrishCitizen, result.Model<PartnerNationalityViewModel>().PartnerNationality);
    }

    [Fact]
    public void PartnerNationality_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.PartnerNationality());
        Assert.NotNull(result.Model<PartnerNationalityViewModel>());
    }
}

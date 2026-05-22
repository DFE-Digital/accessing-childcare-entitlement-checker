using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class UserControllerTests
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _journeyState = new JourneyState();
        _journeySession = Substitute.For<IJourneySession>();
        _controller = new UserController(_journeyState, _journeySession);
    }

    [Fact]
    public void UserAge_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.UserAge());

        Assert.Null(result.Model<UserAgeViewModel>().UserAge);
    }

    [Fact]
    public void UserAge_Get_PopulatesModel_FromState()
    {
        _journeyState.UserAge = AgeRange.UnderEighteen;
        var result = Assert.IsType<ViewResult>(_controller.UserAge());

        Assert.Equal(AgeRange.UnderEighteen, result.Model<UserAgeViewModel>().UserAge);
    }

    [Fact]
    public void UserAge_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new UserAgeViewModel
        {
            UserAge = AgeRange.UnderEighteen
        };

        var result = _controller.UserAge(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(AgeRange.UnderEighteen, _journeyState.UserAge);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(UserController.Nationality), redirect.ActionName);
        Assert.Equal("User", redirect.ControllerName);
    }

    [Fact]
    public void UserAge_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new UserAgeViewModel
        {
            UserAge = null
        };

        _controller.ModelState.AddModelError(nameof(model.UserAge), "Faked Model Binding Error");

        var result = _controller.UserAge(model);

        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.UserAge)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void Nationality_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.Nationality());

        Assert.Null(result.Model<NationalityViewModel>().Nationality);
    }

    [Fact]
    public void Nationality_Get_PopulatesModel_FromState()
    {
        _journeyState.Nationality = NationalityOption.BritishOrIrishCitizen;

        var result = Assert.IsType<ViewResult>(_controller.Nationality());

        Assert.Equal(NationalityOption.BritishOrIrishCitizen, result.Model<NationalityViewModel>().Nationality);
    }

    [Fact]
    public void Nationality_Post_WithCitizenOfAnEUCountryEEACountryOrSwitzerland_SavesState_AndRedirects()
    {
        var model = new NationalityViewModel
        {
            Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland
        };

        var result = _controller.Nationality(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, _journeyState.Nationality);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal("SettledStatus", redirect.ActionName);
        Assert.Equal("User", redirect.ControllerName);
    }

    [Fact]
    public void Nationality_Post_WithFallbackSelection_SavesState_AndRedirects()
    {
        var model = new NationalityViewModel
        {
            Nationality = NationalityOption.BritishOrIrishCitizen
        };

        var result = _controller.Nationality(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(NationalityOption.BritishOrIrishCitizen, _journeyState.Nationality);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal("PaidWork", redirect.ActionName);
        Assert.Equal("User", redirect.ControllerName);
    }

    [Fact]
    public void Nationality_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new NationalityViewModel
        {
            Nationality = null
        };
        _controller.ModelState.AddModelError(nameof(model.Nationality), "Faked Model Binding Error");

        var result = _controller.Nationality(model);

        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.Nationality)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void HasPartner_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.HasPartner());

        Assert.Null(result.Model<HasPartnerViewModel>().HasPartner);
    }

    [Fact]
    public void HasPartner_Get_PopulatesModel_FromState()
    {
        _journeyState.HasPartner = true;
        var result = Assert.IsType<ViewResult>(_controller.HasPartner());

        Assert.Equal(true, result.Model<HasPartnerViewModel>().HasPartner);
    }

    [Fact]
    public void HasPartner_Post_ValidSelection_SavesState_AndRedirects()
    {
        var model = new HasPartnerViewModel
        {
            HasPartner = true
        };

        var result = _controller.HasPartner(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(true, _journeyState.HasPartner);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(PartnerController.PartnerAge), redirect.ActionName);
        Assert.Equal("Partner", redirect.ControllerName);
    }

    [Fact]
    public void HasPartner_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new HasPartnerViewModel
        {
            HasPartner = null
        };

        _controller.ModelState.AddModelError(nameof(model.HasPartner), "Faked Model Binding Error");

        var result = _controller.HasPartner(model);

        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.HasPartner)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void SettledStatus_Post_WithYes_SavesState_AndRedirects()
    {
        var model = new SettledStatusViewModel
        {
            SettledStatus = SettledStatusOption.Yes
        };

        var result = _controller.SettledStatus(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(SettledStatusOption.Yes, _journeyState.SettledStatus);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal("PaidWork", redirect.ActionName);
        Assert.Equal("User", redirect.ControllerName);
    }

    [Fact]
    public void SettledStatus_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new SettledStatusViewModel
        {
            SettledStatus = null
        };
        _controller.ModelState.AddModelError(nameof(model.SettledStatus), "Faked Model Binding Error");

        var result = _controller.SettledStatus(model);

        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.SettledStatus)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void SettledStatus_Get_PopulatesModel_FromState()
    {
        _journeyState.SettledStatus = SettledStatusOption.Yes;

        var result = Assert.IsType<ViewResult>(_controller.SettledStatus());

        Assert.Equal(SettledStatusOption.Yes, result.Model<SettledStatusViewModel>().SettledStatus);
    }

    [Fact]
    public void SettledStatus_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.SettledStatus());

        Assert.Null(result.Model<SettledStatusViewModel>().SettledStatus);
    }

    [Fact]
    public void PaidWork_Post_WithNo_SavesState_AndRedirects()
    {
        var model = new PaidWorkViewModel
        {
            PaidWork = PaidWorkOption.No
        };

        var result = _controller.PaidWork(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(PaidWorkOption.No, _journeyState.PaidWork);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal("UniversalCredit", redirect.ActionName);
        Assert.Equal("User", redirect.ControllerName);
    }

    [Fact]
    public void PaidWork_Post_WithOnLeave_SavesState_AndRedirects()
    {
        var model = new PaidWorkViewModel
        {
            PaidWork = PaidWorkOption.OnLeave
        };

        var result = _controller.PaidWork(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(PaidWorkOption.OnLeave, _journeyState.PaidWork);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal("TypeOfLeave", redirect.ActionName);
        Assert.Equal("User", redirect.ControllerName);
    }

    [Fact]
    public void PaidWork_Post_WithYes_SavesState_AndRedirects()
    {
        var model = new PaidWorkViewModel
        {
            PaidWork = PaidWorkOption.Yes
        };

        var result = _controller.PaidWork(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(PaidWorkOption.Yes, _journeyState.PaidWork);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal("WorkStatus", redirect.ActionName);
        Assert.Equal("User", redirect.ControllerName);
    }

    [Fact]
    public void PaidWork_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new PaidWorkViewModel
        {
            PaidWork = null
        };
        _controller.ModelState.AddModelError(nameof(model.PaidWork), "Faked Model Binding Error");

        var result = _controller.PaidWork(model);

        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PaidWork)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void PaidWork_Get_PopulatesModel_FromState()
    {
        _journeyState.PaidWork = PaidWorkOption.Yes;

        var result = Assert.IsType<ViewResult>(_controller.PaidWork());

        Assert.Equal(PaidWorkOption.Yes, result.Model<PaidWorkViewModel>().PaidWork);
    }

    [Fact]
    public void PaidWork_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.PaidWork());

        Assert.Null(result.Model<PaidWorkViewModel>().PaidWork); main
    }
}

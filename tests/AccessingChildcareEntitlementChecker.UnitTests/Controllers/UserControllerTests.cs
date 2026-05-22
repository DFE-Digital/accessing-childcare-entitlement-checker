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

    [Theory]
    [InlineData(NationalityOption.BritishOrIrishCitizen, "User", nameof(UserController.PaidWork))]
    [InlineData(NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, "User", nameof(UserController.SettledStatus))]
    [InlineData(NationalityOption.CitizenOfADifferentCountry, "User", nameof(UserController.PaidWork))]
    public void Nationality_Post_SavesState_AndRedirects(NationalityOption nationality, string controllerName, string actionName)
    {
        var model = new NationalityViewModel
        {
            Nationality = nationality
        };

        var result = _controller.Nationality(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(nationality, _journeyState.Nationality);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal(controllerName, redirect.ControllerName);
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

        Assert.Null(result.Model<PaidWorkViewModel>().PaidWork);
    }

    [Theory]
    [InlineData(WorkStatusOption.PaidEmployment, "User", nameof(UserController.WeeklyEarnings))]
    [InlineData(WorkStatusOption.SelfEmployed, "User", nameof(UserController.SelfEmployedDuration))]
    [InlineData(WorkStatusOption.Apprentice, "User", nameof(UserController.WeeklyEarnings))]
    public void WorkStatus_Post_SavesState_AndRedirects(WorkStatusOption option, string controllerName, string actionName)
    {
        var model = new WorkStatusViewModel
        {
            WorkStatus = [option]
        };

        var result = _controller.WorkStatus(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal(controllerName, redirect.ControllerName);
    }

    [Fact]
    public void WorkStatus_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new WorkStatusViewModel();
        _controller.ModelState.AddModelError(nameof(model.WorkStatus), "Faked Model Binding Error");
        var result = _controller.WorkStatus(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.WorkStatus)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void WorkStatus_Get_PopulatesModel_FromState()
    {
        _journeyState.WorkStatus = [WorkStatusOption.PaidEmployment];
        var result = Assert.IsType<ViewResult>(_controller.WorkStatus());
        Assert.Equal([WorkStatusOption.PaidEmployment], result.Model<WorkStatusViewModel>().WorkStatus);
    }

    [Fact]
    public void WorkStatus_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.WorkStatus());
        Assert.NotNull(result.Model<WorkStatusViewModel>());
    }

    [Theory]
    [InlineData(SelfEmployedDurationOption.NotLessThan12Months, "User", nameof(UserController.WeeklyEarnings))]
    [InlineData(SelfEmployedDurationOption.LessThan12Months, "User", nameof(UserController.UniversalCredit))]
    public void SelfEmployedDuration_Post_SavesState_AndRedirects(SelfEmployedDurationOption option, string controllerName, string actionName)
    {
        var model = new SelfEmployedDurationViewModel
        {
            SelfEmployedDuration = option
        };
        var result = _controller.SelfEmployedDuration(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal(controllerName, redirect.ControllerName);
    }

    [Fact]
    public void SelfEmployedDuration_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new SelfEmployedDurationViewModel();
        _controller.ModelState.AddModelError(nameof(model.SelfEmployedDuration), "Faked Model Binding Error");
        var result = _controller.SelfEmployedDuration(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.SelfEmployedDuration)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void SelfEmployedDuration_Get_PopulatesModel_FromState()
    {
        _journeyState.SelfEmployedDuration = SelfEmployedDurationOption.LessThan12Months;
        var result = Assert.IsType<ViewResult>(_controller.SelfEmployedDuration());
        Assert.Equal(SelfEmployedDurationOption.LessThan12Months, result.Model<SelfEmployedDurationViewModel>().SelfEmployedDuration);
    }

    [Fact]
    public void SelfEmployedDuration_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.SelfEmployedDuration());
        Assert.NotNull(result.Model<SelfEmployedDurationViewModel>());
    }

    [Theory]
    [InlineData(YearlyEarningsOption.AboveThreshold, "User", nameof(UserController.Benefits))]
    [InlineData(YearlyEarningsOption.BelowThreshold, "User", nameof(UserController.UniversalCredit))]
    public void YearlyEarnings_Post_SavesState_AndRedirects(YearlyEarningsOption option, string controllerName, string actionName)
    {
        var model = new YearlyEarningsViewModel
        {
            YearlyEarnings = option
        };
        var result = _controller.YearlyEarnings(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal(controllerName, redirect.ControllerName);
    }

    [Fact]
    public void YearlyEarnings_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new YearlyEarningsViewModel();
        _controller.ModelState.AddModelError(nameof(model.YearlyEarnings), "Faked Model Binding Error");
        var result = _controller.YearlyEarnings(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.YearlyEarnings)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void YearlyEarnings_Get_PopulatesModel_FromState()
    {
        _journeyState.YearlyEarnings = YearlyEarningsOption.AboveThreshold;
        var result = Assert.IsType<ViewResult>(_controller.YearlyEarnings());
        Assert.Equal(YearlyEarningsOption.AboveThreshold, result.Model<YearlyEarningsViewModel>().YearlyEarnings);
    }

    [Fact]
    public void YearlyEarnings_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.YearlyEarnings());
        Assert.NotNull(result.Model<YearlyEarningsViewModel>());
    }

    [Theory]
    [InlineData(WeeklyEarningsOption.AboveThreshold, "User", nameof(UserController.YearlyEarnings))]
    [InlineData(WeeklyEarningsOption.BelowThreshold, "User", nameof(UserController.UniversalCredit))]
    public void WeeklyEarnings_Post_SavesState_AndRedirects(WeeklyEarningsOption option, string controllerName, string actionName)
    {
        var model = new WeeklyEarningsViewModel
        {
            WeeklyEarnings = option
        };
        var result = _controller.WeeklyEarnings(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal(controllerName, redirect.ControllerName);
    }

    [Fact]
    public void WeeklyEarnings_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new WeeklyEarningsViewModel();
        _controller.ModelState.AddModelError(nameof(model.WeeklyEarnings), "Faked Model Binding Error");
        var result = _controller.WeeklyEarnings(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.WeeklyEarnings)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void WeeklyEarnings_Get_PopulatesModel_FromState()
    {
        _journeyState.WeeklyEarnings = WeeklyEarningsOption.AboveThreshold;
        var result = Assert.IsType<ViewResult>(_controller.WeeklyEarnings());
        Assert.Equal(WeeklyEarningsOption.AboveThreshold, result.Model<WeeklyEarningsViewModel>().WeeklyEarnings);
    }

    [Fact]
    public void WeeklyEarnings_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.WeeklyEarnings());
        Assert.NotNull(result.Model<WeeklyEarningsViewModel>());
    }

    [Theory]
    [InlineData(UniversalCreditOption.Receives, "User", nameof(UserController.Benefits))]
    [InlineData(UniversalCreditOption.DoesNotReceive, "User", nameof(UserController.Benefits))]
    public void UniversalCredit_Post_SavesState_AndRedirects(UniversalCreditOption option, string controllerName, string actionName)
    {
        var model = new UniversalCreditViewModel
        {
            UniversalCredit = option
        };

        var result = _controller.UniversalCredit(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(option, _journeyState.UniversalCredit);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal(controllerName, redirect.ControllerName);
    }

    [Fact]
    public void UniversalCredit_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new UniversalCreditViewModel();
        _controller.ModelState.AddModelError(nameof(model.UniversalCredit), "Faked Model Binding Error");
        var result = _controller.UniversalCredit(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.UniversalCredit)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void UniversalCredit_Get_PopulatesModel_FromState()
    {
        _journeyState.UniversalCredit = UniversalCreditOption.Receives;
        var result = Assert.IsType<ViewResult>(_controller.UniversalCredit());
        Assert.Equal(UniversalCreditOption.Receives, result.Model<UniversalCreditViewModel>().UniversalCredit);
    }

    [Fact]
    public void UniversalCredit_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.UniversalCredit());
        Assert.NotNull(result.Model<UniversalCreditViewModel>());
    }

    [Theory]
    [InlineData(BenefitsOption.CarersAllowance, "User", nameof(UserController.ChildcareSupport))]
    [InlineData(BenefitsOption.ContributionBasedEmploymentAndSupportAllowance, "User", nameof(UserController.ChildcareSupport))]
    [InlineData(BenefitsOption.EmploymentAndSupportAllowance, "User", nameof(UserController.ChildcareSupport))]
    [InlineData(BenefitsOption.GuaranteedElementOfPensionCredit, "User", nameof(UserController.ChildcareSupport))]
    [InlineData(BenefitsOption.IncapacityBenefit, "User", nameof(UserController.ChildcareSupport))]
    [InlineData(BenefitsOption.LimitedCapabilityForWork, "User", nameof(UserController.ChildcareSupport))]
    [InlineData(BenefitsOption.LimitedCapabilityForWorkRelatedActivity, "User", nameof(UserController.ChildcareSupport))]
    [InlineData(BenefitsOption.SevereDisablementAllowance, "User", nameof(UserController.ChildcareSupport))]
    [InlineData(BenefitsOption.None, "User", nameof(UserController.ChildcareSupport))]
    public void Benefits_Post_SavesState_AndRedirects(BenefitsOption option, string controllerName, string actionName)
    {
        var model = new BenefitsViewModel
        {
            Benefits = [option]
        };

        var result = _controller.Benefits(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal([option], _journeyState.Benefits);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal(controllerName, redirect.ControllerName);
    }

    [Fact]
    public void Benefits_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new BenefitsViewModel();
        _controller.ModelState.AddModelError(nameof(model.Benefits), "Faked Model Binding Error");
        var result = _controller.Benefits(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.Benefits)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void Benefits_Get_PopulatesModel_FromState()
    {
        _journeyState.Benefits = [BenefitsOption.CarersAllowance];
        var result = Assert.IsType<ViewResult>(_controller.Benefits());
        Assert.Equal([BenefitsOption.CarersAllowance], result.Model<BenefitsViewModel>().Benefits);
    }

    [Fact]
    public void Benefits_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.Benefits());
        Assert.NotNull(result.Model<BenefitsViewModel>());
    }

    [Theory]
    [InlineData(ChildcareSupportOption.ChildcareVouchers, "User", nameof(UserController.ChildcareVoucherReceipt))]
    [InlineData(ChildcareSupportOption.ChildcareBursaryOrGrant, "User", nameof(UserController.HasPartner))]
    [InlineData(ChildcareSupportOption.None, "User", nameof(UserController.HasPartner))]
    public void ChildcareSupport_Post_SavesState_AndRedirects(ChildcareSupportOption option, string controllerName, string actionName)
    {
        var model = new ChildcareSupportViewModel
        {
            ChildcareSupport = [option]
        };

        var result = _controller.ChildcareSupport(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal([option], _journeyState.ChildcareSupport);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal(controllerName, redirect.ControllerName);
    }

    [Fact]
    public void ChildcareSupport_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new ChildcareSupportViewModel();
        _controller.ModelState.AddModelError(nameof(model.ChildcareSupport), "Faked Model Binding Error");
        var result = _controller.ChildcareSupport(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.ChildcareSupport)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void ChildcareSupport_Get_PopulatesModel_FromState()
    {
        _journeyState.ChildcareSupport = [ChildcareSupportOption.ChildcareVouchers];
        var result = Assert.IsType<ViewResult>(_controller.ChildcareSupport());
        Assert.Equal([ChildcareSupportOption.ChildcareVouchers], result.Model<ChildcareSupportViewModel>().ChildcareSupport);
    }

    [Fact]
    public void ChildcareSupport_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.ChildcareSupport());
        Assert.NotNull(result.Model<ChildcareSupportViewModel>());
    }

    [Theory]
    [InlineData(ChildcareVoucherReceiptOption.WorkplaceNurseryScheme, "User", nameof(UserController.HasPartner))]
    [InlineData(ChildcareVoucherReceiptOption.EmployerArrangesWithProvider, "User", nameof(UserController.HasPartner))]
    [InlineData(ChildcareVoucherReceiptOption.ThroughSalarySacrifice, "User", nameof(UserController.HasPartner))]
    public void ChildcareVoucherReceipt_Post_SavesState_AndRedirects(ChildcareVoucherReceiptOption option, string controllerName, string actionName)
    {
        var model = new ChildcareVoucherReceiptViewModel
        {
            ChildcareVoucherReceipt = option
        };
        var result = _controller.ChildcareVoucherReceipt(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(option, _journeyState.ChildcareVoucherReceipt);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal(controllerName, redirect.ControllerName);
    }

    [Fact]
    public void ChildcareVoucherReceipt_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new ChildcareVoucherReceiptViewModel();
        _controller.ModelState.AddModelError(nameof(model.ChildcareVoucherReceipt), "Faked Model Binding Error");
        var result = _controller.ChildcareVoucherReceipt(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.ChildcareVoucherReceipt)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void ChildcareVoucherReceipt_Get_PopulatesModel_FromState()
    {
        _journeyState.ChildcareVoucherReceipt = ChildcareVoucherReceiptOption.WorkplaceNurseryScheme;
        var result = Assert.IsType<ViewResult>(_controller.ChildcareVoucherReceipt());
        Assert.Equal(ChildcareVoucherReceiptOption.WorkplaceNurseryScheme, result.Model<ChildcareVoucherReceiptViewModel>().ChildcareVoucherReceipt);
    }

    [Fact]
    public void ChildcareVoucherReceipt_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.ChildcareVoucherReceipt());
        Assert.NotNull(result.Model<ChildcareVoucherReceiptViewModel>());
    }
}

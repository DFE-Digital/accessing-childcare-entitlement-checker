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

    [Theory]
    [InlineData(SettledStatusOption.Yes, "Partner", nameof(PartnerController.PartnerPaidWork))]
    [InlineData(SettledStatusOption.No, "Partner", nameof(PartnerController.PartnerPaidWork))]
    [InlineData(SettledStatusOption.StillWaiting, "Partner", nameof(PartnerController.PartnerPaidWork))]
    public void PartnerSettledStatus_Post_SavesState_AndRedirects(SettledStatusOption option, string controllerName, string actionName)
    {
        var model = new PartnerSettledStatusViewModel
        {
            PartnerSettledStatus = option
        };
        var result = _controller.PartnerSettledStatus(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(option, _journeyState.PartnerSettledStatus);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal(controllerName, redirect.ControllerName);
    }

    [Fact]
    public void PartnerSettledStatus_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new PartnerSettledStatusViewModel();
        _controller.ModelState.AddModelError(nameof(model.PartnerSettledStatus), "Faked Model Binding Error");
        var result = _controller.PartnerSettledStatus(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PartnerSettledStatus)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void PartnerSettledStatus_Get_PopulatesModel_FromState()
    {
        _journeyState.PartnerSettledStatus = SettledStatusOption.Yes;
        var result = Assert.IsType<ViewResult>(_controller.PartnerSettledStatus());
        Assert.Equal(SettledStatusOption.Yes, result.Model<PartnerSettledStatusViewModel>().PartnerSettledStatus);
    }

    [Fact]
    public void PartnerSettledStatus_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.PartnerSettledStatus());
        Assert.NotNull(result.Model<PartnerSettledStatusViewModel>());
    }

    [Theory]
    [InlineData(PartnerPaidWorkOption.Yes, "Partner", nameof(PartnerController.PartnerWorkStatus))]
    [InlineData(PartnerPaidWorkOption.No, "Partner", nameof(PartnerController.PartnerBenefits))]
    [InlineData(PartnerPaidWorkOption.OnLeave, "Partner", nameof(PartnerController.PartnerTypeOfLeave))]
    public void PartnerPaidWork_Post_SavesState_AndRedirects(PartnerPaidWorkOption option, string controllerName, string actionName)
    {
        var model = new PartnerPaidWorkViewModel
        {
            PartnerPaidWork = option
        };
        var result = _controller.PartnerPaidWork(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(option, _journeyState.PartnerPaidWork);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal(controllerName, redirect.ControllerName);
    }

    [Fact]
    public void PartnerPaidWork_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new PartnerPaidWorkViewModel();
        _controller.ModelState.AddModelError(nameof(model.PartnerPaidWork), "Faked Model Binding Error");
        var result = _controller.PartnerPaidWork(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PartnerPaidWork)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void PartnerPaidWork_Get_PopulatesModel_FromState()
    {
        _journeyState.PartnerPaidWork = PartnerPaidWorkOption.Yes;
        var result = Assert.IsType<ViewResult>(_controller.PartnerPaidWork());
        Assert.Equal(PartnerPaidWorkOption.Yes, result.Model<PartnerPaidWorkViewModel>().PartnerPaidWork);
    }

    [Fact]
    public void PartnerPaidWork_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.PartnerPaidWork());
        Assert.NotNull(result.Model<PartnerPaidWorkViewModel>());
    }

    [Theory]
    [InlineData(WorkStatusOption.PaidEmployment, "Partner", nameof(PartnerController.PartnerWeeklyEarnings))]
    [InlineData(WorkStatusOption.SelfEmployed, "Partner", nameof(PartnerController.PartnerSelfEmployedDuration))]
    [InlineData(WorkStatusOption.Apprentice, "Partner", nameof(PartnerController.PartnerWeeklyEarnings))]
    public void PartnerWorkStatus_Post_SavesState_AndRedirects(WorkStatusOption option, string controllerName, string actionName)
    {
        var model = new PartnerWorkStatusViewModel
        {
            PartnerWorkStatus = [option]
        };
        var result = _controller.PartnerWorkStatus(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal([option], _journeyState.PartnerWorkStatus);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal(controllerName, redirect.ControllerName);
    }

    [Fact]
    public void PartnerWorkStatus_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new PartnerWorkStatusViewModel();
        _controller.ModelState.AddModelError(nameof(model.PartnerWorkStatus), "Faked Model Binding Error");
        var result = _controller.PartnerWorkStatus(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PartnerWorkStatus)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void PartnerWorkStatus_Get_PopulatesModel_FromState()
    {
        _journeyState.PartnerWorkStatus = [WorkStatusOption.PaidEmployment];
        var result = Assert.IsType<ViewResult>(_controller.PartnerWorkStatus());
        Assert.Equal([WorkStatusOption.PaidEmployment], result.Model<PartnerWorkStatusViewModel>().PartnerWorkStatus);
    }

    [Fact]
    public void PartnerWorkStatus_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.PartnerWorkStatus());
        Assert.NotNull(result.Model<PartnerWorkStatusViewModel>());
    }

    [Theory]
    [InlineData(PartnerBenefitsOption.CarersAllowance, "Partner", nameof(PartnerController.PartnerChildcareSupport))]
    [InlineData(PartnerBenefitsOption.ContributionBasedEmploymentAndSupportAllowance, "Partner", nameof(PartnerController.PartnerChildcareSupport))]
    [InlineData(PartnerBenefitsOption.EmploymentAndSupportAllowance, "Partner", nameof(PartnerController.PartnerChildcareSupport))]
    [InlineData(PartnerBenefitsOption.GuaranteedElementOfPensionCredit, "Partner", nameof(PartnerController.PartnerChildcareSupport))]
    [InlineData(PartnerBenefitsOption.IncapacityBenefit, "Partner", nameof(PartnerController.PartnerChildcareSupport))]
    [InlineData(PartnerBenefitsOption.LimitedCapabilityForWork, "Partner", nameof(PartnerController.PartnerChildcareSupport))]
    [InlineData(PartnerBenefitsOption.LimitedCapabilityForWorkRelatedActivity, "Partner", nameof(PartnerController.PartnerChildcareSupport))]
    [InlineData(PartnerBenefitsOption.SevereDisablementAllowance, "Partner", nameof(PartnerController.PartnerChildcareSupport))]
    [InlineData(PartnerBenefitsOption.None, "Partner", nameof(PartnerController.PartnerChildcareSupport))]
    public void PartnerBenefits_Post_SavesState_AndRedirects(PartnerBenefitsOption option, string controllerName, string actionName)
    {
        var model = new PartnerBenefitsViewModel
        {
            PartnerBenefits = [option]
        };
        var result = _controller.PartnerBenefits(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal([option], _journeyState.PartnerBenefits);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal(controllerName, redirect.ControllerName);
    }

    [Fact]
    public void PartnerBenefits_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new PartnerBenefitsViewModel();
        _controller.ModelState.AddModelError(nameof(model.PartnerBenefits), "Faked Model Binding Error");
        var result = _controller.PartnerBenefits(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PartnerBenefits)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void PartnerBenefits_Get_PopulatesModel_FromState()
    {
        _journeyState.PartnerBenefits = [PartnerBenefitsOption.CarersAllowance];
        var result = Assert.IsType<ViewResult>(_controller.PartnerBenefits());
        Assert.Equal([PartnerBenefitsOption.CarersAllowance], result.Model<PartnerBenefitsViewModel>().PartnerBenefits);
    }

    [Fact]
    public void PartnerBenefits_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.PartnerBenefits());
        Assert.NotNull(result.Model<PartnerBenefitsViewModel>());
    }

    [Theory]
    [InlineData(SelfEmployedDurationOption.LessThan12Months, "Partner", nameof(PartnerController.PartnerBenefits))]
    [InlineData(SelfEmployedDurationOption.NotLessThan12Months, "Partner", nameof(PartnerController.PartnerWeeklyEarnings))]
    public void PartnerSelfEmployedDuration_Post_SavesState_AndRedirects(SelfEmployedDurationOption option, string controllerName, string actionName)
    {
        var model = new PartnerSelfEmployedDurationViewModel
        {
            PartnerSelfEmployedDuration = option
        };
        var result = _controller.PartnerSelfEmployedDuration(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(option, _journeyState.PartnerSelfEmployedDuration);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal(controllerName, redirect.ControllerName);
    }

    [Fact]
    public void PartnerSelfEmployedDuration_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new PartnerSelfEmployedDurationViewModel();
        _controller.ModelState.AddModelError(nameof(model.PartnerSelfEmployedDuration), "Faked Model Binding Error");
        var result = _controller.PartnerSelfEmployedDuration(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PartnerSelfEmployedDuration)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void PartnerSelfEmployedDuration_Get_PopulatesModel_FromState()
    {
        _journeyState.PartnerSelfEmployedDuration = SelfEmployedDurationOption.LessThan12Months;
        var result = Assert.IsType<ViewResult>(_controller.PartnerSelfEmployedDuration());
        Assert.Equal(SelfEmployedDurationOption.LessThan12Months, result.Model<PartnerSelfEmployedDurationViewModel>().PartnerSelfEmployedDuration);
    }

    [Fact]
    public void PartnerSelfEmployedDuration_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.PartnerSelfEmployedDuration());
        Assert.NotNull(result.Model<PartnerSelfEmployedDurationViewModel>());
    }

    [Theory]
    [InlineData(WeeklyEarningsOption.AboveThreshold, "Partner", nameof(PartnerController.PartnerYearlyEarnings))]
    [InlineData(WeeklyEarningsOption.BelowThreshold, "Partner", nameof(PartnerController.PartnerBenefits))]
    public void PartnerWeeklyEarnings_Post_SavesState_AndRedirects(WeeklyEarningsOption option, string controllerName, string actionName)
    {
        var model = new PartnerWeeklyEarningsViewModel
        {
            PartnerWeeklyEarnings = option
        };
        var result = _controller.PartnerWeeklyEarnings(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(option, _journeyState.PartnerWeeklyEarnings);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal(controllerName, redirect.ControllerName);
    }

    [Fact]
    public void PartnerWeeklyEarnings_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new PartnerWeeklyEarningsViewModel();
        _controller.ModelState.AddModelError(nameof(model.PartnerWeeklyEarnings), "Faked Model Binding Error");
        var result = _controller.PartnerWeeklyEarnings(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PartnerWeeklyEarnings)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void PartnerWeeklyEarnings_Get_PopulatesModel_FromState()
    {
        _journeyState.PartnerWeeklyEarnings = WeeklyEarningsOption.AboveThreshold;
        var result = Assert.IsType<ViewResult>(_controller.PartnerWeeklyEarnings());
        Assert.Equal(WeeklyEarningsOption.AboveThreshold, result.Model<PartnerWeeklyEarningsViewModel>().PartnerWeeklyEarnings);
    }

    [Fact]
    public void PartnerWeeklyEarnings_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.PartnerWeeklyEarnings());
        Assert.NotNull(result.Model<PartnerWeeklyEarningsViewModel>());
    }

    [Theory]
    [InlineData(YearlyEarningsOption.AboveThreshold, "Partner", nameof(PartnerController.PartnerBenefits))]
    [InlineData(YearlyEarningsOption.BelowThreshold, "Partner", nameof(PartnerController.PartnerBenefits))]
    public void PartnerYearlyEarnings_Post_SavesState_AndRedirects(YearlyEarningsOption option, string controllerName, string actionName)
    {
        var model = new PartnerYearlyEarningsViewModel
        {
            PartnerYearlyEarnings = option
        };
        var result = _controller.PartnerYearlyEarnings(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(option, _journeyState.PartnerYearlyEarnings);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal(controllerName, redirect.ControllerName);
    }

    [Fact]
    public void PartnerYearlyEarnings_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new PartnerYearlyEarningsViewModel();
        _controller.ModelState.AddModelError(nameof(model.PartnerYearlyEarnings), "Faked Model Binding Error");
        var result = _controller.PartnerYearlyEarnings(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PartnerYearlyEarnings)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void PartnerYearlyEarnings_Get_PopulatesModel_FromState()
    {
        _journeyState.PartnerYearlyEarnings = YearlyEarningsOption.AboveThreshold;
        var result = Assert.IsType<ViewResult>(_controller.PartnerYearlyEarnings());
        Assert.Equal(YearlyEarningsOption.AboveThreshold, result.Model<PartnerYearlyEarningsViewModel>().PartnerYearlyEarnings);
    }

    [Fact]
    public void PartnerYearlyEarnings_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.PartnerYearlyEarnings());
        Assert.NotNull(result.Model<PartnerYearlyEarningsViewModel>());
    }

    [Theory]
    [InlineData(PartnerChildcareSupportOption.ChildcareVouchers, "Partner", nameof(PartnerController.PartnerChildcareVoucherReceipt))]
    [InlineData(PartnerChildcareSupportOption.ChildcareBursaryOrGrant, "Summary", nameof(SummaryController.CheckAnswers))]
    [InlineData(PartnerChildcareSupportOption.None, "Summary", nameof(SummaryController.CheckAnswers))]
    public void PartnerChildcareSupport_Post_SavesState_AndRedirects(PartnerChildcareSupportOption option, string controllerName, string actionName)
    {
        var model = new PartnerChildcareSupportViewModel
        {
            PartnerChildcareSupport = [option]
        };
        var result = _controller.PartnerChildcareSupport(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal([option], _journeyState.PartnerChildcareSupport);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal(controllerName, redirect.ControllerName);
    }

    [Fact]
    public void PartnerChildcareSupport_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new PartnerChildcareSupportViewModel();
        _controller.ModelState.AddModelError(nameof(model.PartnerChildcareSupport), "Faked Model Binding Error");
        var result = _controller.PartnerChildcareSupport(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PartnerChildcareSupport)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void PartnerChildcareSupport_Get_PopulatesModel_FromState()
    {
        _journeyState.PartnerChildcareSupport = [PartnerChildcareSupportOption.ChildcareVouchers];
        var result = Assert.IsType<ViewResult>(_controller.PartnerChildcareSupport());
        Assert.Equal([PartnerChildcareSupportOption.ChildcareVouchers], result.Model<PartnerChildcareSupportViewModel>().PartnerChildcareSupport);
    }

    [Fact]
    public void PartnerChildcareSupport_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.PartnerChildcareSupport());
        Assert.NotNull(result.Model<PartnerChildcareSupportViewModel>());
    }

    [Theory]
    [InlineData(ChildcareVoucherReceiptOption.WorkplaceNurseryScheme, "Summary", nameof(SummaryController.CheckAnswers))]
    [InlineData(ChildcareVoucherReceiptOption.EmployerArrangesWithProvider, "Summary", nameof(SummaryController.CheckAnswers))]
    [InlineData(ChildcareVoucherReceiptOption.ThroughSalarySacrifice, "Summary", nameof(SummaryController.CheckAnswers))]
    public void PartnerChildcareVoucherReceipt_Post_SavesState_AndRedirects(ChildcareVoucherReceiptOption option, string controllerName, string actionName)
    {
        var model = new PartnerChildcareVoucherReceiptViewModel
        {
            PartnerChildcareVoucherReceipt = option
        };
        var result = _controller.PartnerChildcareVoucherReceipt(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).Set(_journeyState);
        Assert.Equal(option, _journeyState.PartnerChildcareVoucherReceipt);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal(controllerName, redirect.ControllerName);
    }

    [Fact]
    public void PartnerChildcareVoucherReceipt_Post_InvalidSelection_ReturnsViewWithError()
    {
        var model = new PartnerChildcareVoucherReceiptViewModel();
        _controller.ModelState.AddModelError(nameof(model.PartnerChildcareVoucherReceipt), "Faked Model Binding Error");
        var result = _controller.PartnerChildcareVoucherReceipt(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PartnerChildcareVoucherReceipt)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void PartnerChildcareVoucherReceipt_Get_PopulatesModel_FromState()
    {
        _journeyState.PartnerChildcareVoucherReceipt = ChildcareVoucherReceiptOption.WorkplaceNurseryScheme;
        var result = Assert.IsType<ViewResult>(_controller.PartnerChildcareVoucherReceipt());
        Assert.Equal(ChildcareVoucherReceiptOption.WorkplaceNurseryScheme, result.Model<PartnerChildcareVoucherReceiptViewModel>().PartnerChildcareVoucherReceipt);
    }

    [Fact]
    public void PartnerChildcareVoucherReceipt_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.PartnerChildcareVoucherReceipt());
        Assert.NotNull(result.Model<PartnerChildcareVoucherReceiptViewModel>());
    }
}

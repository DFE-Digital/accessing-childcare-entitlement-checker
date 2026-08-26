using System;
using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using NSubstitute;
using System.Diagnostics;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class PartnerControllerTests : IDisposable
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly PartnerController _controller;

    public PartnerControllerTests()
    {
        _journeyState = new JourneyState();
        _journeySession = Substitute.For<IJourneySession>();
        _controller = new PartnerController(_journeyState, _journeySession);
        _controller.Url = Substitute.For<IUrlHelper>();
        _controller.Url.Action(Arg.Any<UrlActionContext>()).Returns("backlink");
    }

    [Fact]
    public void PartnerAgeReturnsView()
    {
        var result = _controller.PartnerAge();
        Assert.Null(result.Model<PartnerAgeViewModel>().PartnerAge);
    }

    [Fact]
    public void PartnerAgeGetPopulatesModelFromState()
    {
        _journeyState.PartnerAge = AgeRange.EighteenToTwenty;
        var result = _controller.PartnerAge();
        Assert.Equal(AgeRange.EighteenToTwenty, result.Model<PartnerAgeViewModel>().PartnerAge);
    }

    [Theory]
    [InlineData(null, NationalityOption.CitizenOfADifferentCountry, nameof(PartnerController.PartnerNationality))]
    [InlineData(null, NationalityOption.BritishOrIrishCitizen, nameof(PartnerController.PartnerPaidWork))]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.CitizenOfADifferentCountry, nameof(PartnerController.PartnerNationality))]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.BritishOrIrishCitizen, nameof(PartnerController.PartnerPaidWork))]
    public void PartnerAgePostValidSelectionSavesStateAndRedirects(string? returnTo, NationalityOption userNationality, string actionName)
    {
        _journeyState.PartnerAge = AgeRange.EighteenToTwenty;
        _journeyState.Nationality = userNationality;
        _journeyState.PartnerPaidWork = PartnerPaidWorkOption.Yes;
        _journeyState.PartnerWeeklyEarnings = WeeklyEarningsOption.BelowThreshold;
        var model = new PartnerAgeViewModel()
        {
            PartnerAge = AgeRange.EighteenToTwenty,
            ReturnTo = returnTo,
        };

        var result = _controller.PartnerAge(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).SetState(_journeyState);
        Assert.Equal(AgeRange.EighteenToTwenty, _journeyState.PartnerAge);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
    }

    [Fact]
    public void PartnerAgePostInvalidSelectionReturnsViewWithError()
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
    [InlineData(NationalityOption.BritishOrIrishCitizen, null, nameof(PartnerController.PartnerPaidWork))]
    [InlineData(NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, null, nameof(PartnerController.PartnerSettledStatus))]
    [InlineData(NationalityOption.CitizenOfADifferentCountry, null, nameof(PartnerController.PartnerPaidWork))]
    [InlineData(NationalityOption.BritishOrIrishCitizen, ReturnTo.CheckAnswers, nameof(PartnerController.PartnerPaidWork))]
    [InlineData(NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, ReturnTo.CheckAnswers, nameof(PartnerController.PartnerSettledStatus))]
    [InlineData(NationalityOption.CitizenOfADifferentCountry, ReturnTo.CheckAnswers, nameof(PartnerController.PartnerPaidWork))]
    public void PartnerNationalityPostSavesStateAndRedirects(NationalityOption option, string? returnTo, string actionName)
    {
        _journeyState.PartnerNationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland;
        _journeyState.PartnerPaidWork = PartnerPaidWorkOption.Yes;
        _journeyState.PartnerSettledStatus = SettledStatusOption.Yes;
        var model = new PartnerNationalityViewModel
        {
            PartnerNationality = option,
            ReturnTo = returnTo
        };

        var result = _controller.PartnerNationality(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).SetState(_journeyState);
        Assert.Equal(option, _journeyState.PartnerNationality);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
    }

    [Fact]
    public void PartnerNationalityPostInvalidSelectionReturnsViewWithError()
    {
        var model = new PartnerNationalityViewModel();
        _controller.ModelState.AddModelError(nameof(model.PartnerNationality), "Faked Model Binding Error");
        var result = _controller.PartnerNationality(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PartnerNationality)));
        _journeySession.DidNotReceive().SetState(_journeyState);
    }

    [Fact]
    public void PartnerNationalityGetPopulatesModelFromState()
    {
        _journeyState.PartnerNationality = NationalityOption.BritishOrIrishCitizen;
        var result = Assert.IsType<ViewResult>(_controller.PartnerNationality());
        Assert.Equal(NationalityOption.BritishOrIrishCitizen, result.Model<PartnerNationalityViewModel>().PartnerNationality);
    }

    [Fact]
    public void PartnerNationalityReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.PartnerNationality());
        Assert.NotNull(result.Model<PartnerNationalityViewModel>());
    }

    [Fact]
    public void PartnerNationalityPostUnreachableCoverage()
    {
        var model = new PartnerNationalityViewModel
        {
            PartnerNationality = (NationalityOption)99,
        };

        Assert.Throws<UnreachableException>(() => _controller.PartnerNationality(model));
    }

    [Theory]
    [InlineData(SettledStatusOption.Yes, null, nameof(PartnerController.PartnerPaidWork))]
    [InlineData(SettledStatusOption.No, null, nameof(PartnerController.PartnerPaidWork))]
    [InlineData(SettledStatusOption.StillWaiting, null, nameof(PartnerController.PartnerPaidWork))]
    [InlineData(SettledStatusOption.Yes, ReturnTo.CheckAnswers, nameof(PartnerController.PartnerPaidWork))]
    public void PartnerSettledStatusPostSavesStateAndRedirects(SettledStatusOption option, string? returnTo, string actionName)
    {
        _journeyState.PartnerPaidWork = PartnerPaidWorkOption.Yes;
        var model = new PartnerSettledStatusViewModel
        {
            PartnerSettledStatus = option,
            ReturnTo = returnTo
        };
        var result = _controller.PartnerSettledStatus(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).SetState(_journeyState);
        Assert.Equal(option, _journeyState.PartnerSettledStatus);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
    }

    [Fact]
    public void PartnerSettledStatusPostInvalidSelectionReturnsViewWithError()
    {
        var model = new PartnerSettledStatusViewModel();
        _controller.ModelState.AddModelError(nameof(model.PartnerSettledStatus), "Faked Model Binding Error");
        var result = _controller.PartnerSettledStatus(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PartnerSettledStatus)));
        _journeySession.DidNotReceive().SetState(_journeyState);
    }

    [Fact]
    public void PartnerSettledStatusGetPopulatesModelFromState()
    {
        _journeyState.PartnerSettledStatus = SettledStatusOption.Yes;
        var result = Assert.IsType<ViewResult>(_controller.PartnerSettledStatus());
        Assert.Equal(SettledStatusOption.Yes, result.Model<PartnerSettledStatusViewModel>().PartnerSettledStatus);
    }

    [Fact]
    public void PartnerSettledStatusReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.PartnerSettledStatus());
        Assert.NotNull(result.Model<PartnerSettledStatusViewModel>());
    }

    [Theory]
    [InlineData(PartnerPaidWorkOption.Yes, null, nameof(PartnerController.PartnerWorkStatus))]
    [InlineData(PartnerPaidWorkOption.No, null, nameof(PartnerController.PartnerBenefits))]
    [InlineData(PartnerPaidWorkOption.ParentalLeave, null, nameof(PartnerController.PartnerParentalLeave))]
    [InlineData(PartnerPaidWorkOption.Yes, ReturnTo.CheckAnswers, nameof(PartnerController.PartnerWorkStatus))]
    public void PartnerPaidWorkPostSavesStateAndRedirects(PartnerPaidWorkOption option, string? returnTo, string actionName)
    {
        _journeyState.PartnerWorkStatus = [WorkStatusOption.PaidEmployment];
        var model = new PartnerPaidWorkViewModel
        {
            PartnerPaidWork = option,
            ReturnTo = returnTo
        };
        var result = _controller.PartnerPaidWork(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).SetState(_journeyState);
        Assert.Equal(option, _journeyState.PartnerPaidWork);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
    }

    [Fact]
    public void PartnerPaidWorkPostInvalidSelectionReturnsViewWithError()
    {
        var model = new PartnerPaidWorkViewModel();
        _controller.ModelState.AddModelError(nameof(model.PartnerPaidWork), "Faked Model Binding Error");
        var result = _controller.PartnerPaidWork(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PartnerPaidWork)));
        _journeySession.DidNotReceive().SetState(_journeyState);
    }

    [Fact]
    public void PartnerPaidWorkGetPopulatesModelFromState()
    {
        _journeyState.PartnerPaidWork = PartnerPaidWorkOption.Yes;
        var result = Assert.IsType<ViewResult>(_controller.PartnerPaidWork());
        Assert.Equal(PartnerPaidWorkOption.Yes, result.Model<PartnerPaidWorkViewModel>().PartnerPaidWork);
    }

    [Fact]
    public void PartnerPaidWorkReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.PartnerPaidWork());
        Assert.NotNull(result.Model<PartnerPaidWorkViewModel>());
    }

    [Fact]
    public void PartnerPaidWorkPostUnreachableCoverage()
    {
        var model = new PartnerPaidWorkViewModel
        {
            PartnerPaidWork = (PartnerPaidWorkOption)99,
        };

        Assert.Throws<UnreachableException>(() => _controller.PartnerPaidWork(model));
    }

    [Theory]
    [InlineData(WorkStatusOption.PaidEmployment, null, nameof(PartnerController.PartnerWeeklyEarnings))]
    [InlineData(WorkStatusOption.SelfEmployed, null, nameof(PartnerController.PartnerSelfEmployedDuration))]
    [InlineData(WorkStatusOption.Apprentice, null, nameof(PartnerController.PartnerWeeklyEarnings))]
    [InlineData(WorkStatusOption.Apprentice, ReturnTo.CheckAnswers, nameof(PartnerController.PartnerWeeklyEarnings))]
    public void PartnerWorkStatusPostSavesStateAndRedirects(WorkStatusOption option, string? returnTo, string actionName)
    {
        _journeyState.PartnerWeeklyEarnings = WeeklyEarningsOption.BelowThreshold;
        var model = new PartnerWorkStatusViewModel
        {
            PartnerWorkStatus = [option],
            ReturnTo = returnTo
        };
        var result = _controller.PartnerWorkStatus(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).SetState(_journeyState);
        Assert.Equal([option], _journeyState.PartnerWorkStatus);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
    }

    [Fact]
    public void PartnerWorkStatusPostInvalidSelectionReturnsViewWithError()
    {
        var model = new PartnerWorkStatusViewModel();
        _controller.ModelState.AddModelError(nameof(model.PartnerWorkStatus), "Faked Model Binding Error");
        var result = _controller.PartnerWorkStatus(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PartnerWorkStatus)));
        _journeySession.DidNotReceive().SetState(_journeyState);
    }

    [Fact]
    public void PartnerWorkStatusGetPopulatesModelFromState()
    {
        _journeyState.PartnerWorkStatus = [WorkStatusOption.PaidEmployment];
        var result = Assert.IsType<ViewResult>(_controller.PartnerWorkStatus());
        Assert.Equal([WorkStatusOption.PaidEmployment], result.Model<PartnerWorkStatusViewModel>().PartnerWorkStatus);
    }

    [Fact]
    public void PartnerWorkStatusReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.PartnerWorkStatus());
        Assert.NotNull(result.Model<PartnerWorkStatusViewModel>());
    }

    [Theory]
    [InlineData(PartnerBenefitsOption.CarersAllowance, null, nameof(PartnerController.PartnerChildcareSupport))]
    [InlineData(PartnerBenefitsOption.ContributionBasedEmploymentAndSupportAllowance, null, nameof(PartnerController.PartnerChildcareSupport))]
    [InlineData(PartnerBenefitsOption.EmploymentAndSupportAllowance, null, nameof(PartnerController.PartnerChildcareSupport))]
    [InlineData(PartnerBenefitsOption.GuaranteedElementOfPensionCredit, null, nameof(PartnerController.PartnerChildcareSupport))]
    [InlineData(PartnerBenefitsOption.IncapacityBenefit, null, nameof(PartnerController.PartnerChildcareSupport))]
    [InlineData(PartnerBenefitsOption.LimitedCapabilityForWork, null, nameof(PartnerController.PartnerChildcareSupport))]
    [InlineData(PartnerBenefitsOption.LimitedCapabilityForWorkRelatedActivity, null, nameof(PartnerController.PartnerChildcareSupport))]
    [InlineData(PartnerBenefitsOption.SevereDisablementAllowance, null, nameof(PartnerController.PartnerChildcareSupport))]
    [InlineData(PartnerBenefitsOption.None, null, nameof(PartnerController.PartnerChildcareSupport))]
    [InlineData(PartnerBenefitsOption.None, ReturnTo.CheckAnswers, nameof(PartnerController.PartnerChildcareSupport))]
    public void PartnerBenefitsPostSavesStateAndRedirects(PartnerBenefitsOption option, string? returnTo, string actionName)
    {
        _journeyState.PartnerChildcareSupport = [PartnerChildcareSupportOption.ChildcareVouchers];
        var model = new PartnerBenefitsViewModel
        {
            PartnerBenefits = [option],
            ReturnTo = returnTo,
        };

        var result = _controller.PartnerBenefits(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).SetState(_journeyState);
        Assert.Equal([option], _journeyState.PartnerBenefits);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
    }

    [Fact]
    public void PartnerBenefitsPostInvalidSelectionReturnsViewWithError()
    {
        var model = new PartnerBenefitsViewModel();
        _controller.ModelState.AddModelError(nameof(model.PartnerBenefits), "Faked Model Binding Error");
        var result = _controller.PartnerBenefits(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PartnerBenefits)));
        _journeySession.DidNotReceive().SetState(_journeyState);
    }

    [Fact]
    public void PartnerBenefitsGetPopulatesModelFromState()
    {
        _journeyState.PartnerBenefits = [PartnerBenefitsOption.CarersAllowance];
        var result = Assert.IsType<ViewResult>(_controller.PartnerBenefits());
        Assert.Equal([PartnerBenefitsOption.CarersAllowance], result.Model<PartnerBenefitsViewModel>().PartnerBenefits);
    }

    [Fact]
    public void PartnerBenefitsReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.PartnerBenefits());
        Assert.NotNull(result.Model<PartnerBenefitsViewModel>());
    }

    [Theory]
    [InlineData(SelfEmployedDurationOption.LessThan12Months, null, nameof(PartnerController.PartnerBenefits))]
    [InlineData(SelfEmployedDurationOption.NotLessThan12Months, null, nameof(PartnerController.PartnerWeeklyEarnings))]
    [InlineData(SelfEmployedDurationOption.NotLessThan12Months, ReturnTo.CheckAnswers, nameof(PartnerController.PartnerWeeklyEarnings))]
    public void PartnerSelfEmployedDurationPostSavesStateAndRedirects(SelfEmployedDurationOption option, string? returnTo, string actionName)
    {
        _journeyState.PartnerWeeklyEarnings = WeeklyEarningsOption.BelowThreshold;
        var model = new PartnerSelfEmployedDurationViewModel
        {
            PartnerSelfEmployedDuration = option,
            ReturnTo = returnTo,
        };

        var result = _controller.PartnerSelfEmployedDuration(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).SetState(_journeyState);
        Assert.Equal(option, _journeyState.PartnerSelfEmployedDuration);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
    }

    [Fact]
    public void PartnerSelfEmployedDurationPostInvalidSelectionReturnsViewWithError()
    {
        var model = new PartnerSelfEmployedDurationViewModel();
        _controller.ModelState.AddModelError(nameof(model.PartnerSelfEmployedDuration), "Faked Model Binding Error");
        var result = _controller.PartnerSelfEmployedDuration(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PartnerSelfEmployedDuration)));
        _journeySession.DidNotReceive().SetState(_journeyState);
    }

    [Fact]
    public void PartnerSelfEmployedDurationGetPopulatesModelFromState()
    {
        _journeyState.PartnerSelfEmployedDuration = SelfEmployedDurationOption.LessThan12Months;
        var result = Assert.IsType<ViewResult>(_controller.PartnerSelfEmployedDuration());
        Assert.Equal(SelfEmployedDurationOption.LessThan12Months, result.Model<PartnerSelfEmployedDurationViewModel>().PartnerSelfEmployedDuration);
    }

    [Fact]
    public void PartnerSelfEmployedDurationReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.PartnerSelfEmployedDuration());
        Assert.NotNull(result.Model<PartnerSelfEmployedDurationViewModel>());
    }

    [Theory]
    [InlineData(WeeklyEarningsOption.AboveThreshold, null, nameof(PartnerController.PartnerYearlyEarnings))]
    [InlineData(WeeklyEarningsOption.BelowThreshold, null, nameof(PartnerController.PartnerBenefits))]
    [InlineData(WeeklyEarningsOption.AboveThreshold, ReturnTo.CheckAnswers, nameof(PartnerController.PartnerYearlyEarnings))]
    public void PartnerWeeklyEarningsPostSavesStateAndRedirects(WeeklyEarningsOption option, string? returnTo, string actionName)
    {
        _journeyState.PartnerYearlyEarnings = YearlyEarningsOption.BelowThreshold;
        var model = new PartnerWeeklyEarningsViewModel
        {
            PartnerWeeklyEarnings = option,
            ReturnTo = returnTo
        };
        var result = _controller.PartnerWeeklyEarnings(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).SetState(_journeyState);
        Assert.Equal(option, _journeyState.PartnerWeeklyEarnings);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
    }

    [Fact]
    public void PartnerWeeklyEarningsPostInvalidSelectionReturnsViewWithError()
    {
        _journeyState.PartnerAge = AgeRange.UnderEighteen;
        _journeyState.PartnerWorkStatus = [WorkStatusOption.PaidEmployment];

        var model = new PartnerWeeklyEarningsViewModel();
        _controller.ModelState.AddModelError(nameof(model.PartnerWeeklyEarnings), "Faked Model Binding Error");
        var result = _controller.PartnerWeeklyEarnings(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PartnerWeeklyEarnings)));
        _journeySession.DidNotReceive().SetState(_journeyState);
    }

    [Fact]
    public void PartnerWeeklyEarningsGetPopulatesModelFromState()
    {
        _journeyState.PartnerAge = AgeRange.UnderEighteen;
        _journeyState.PartnerWorkStatus = [WorkStatusOption.PaidEmployment];
        _journeyState.PartnerWeeklyEarnings = WeeklyEarningsOption.AboveThreshold;
        var result = Assert.IsType<ViewResult>(_controller.PartnerWeeklyEarnings());
        Assert.Equal(WeeklyEarningsOption.AboveThreshold, result.Model<PartnerWeeklyEarningsViewModel>().PartnerWeeklyEarnings);
    }

    [Fact]
    public void PartnerWeeklyEarningsReturnsView()
    {
        _journeyState.PartnerAge = AgeRange.UnderEighteen;
        _journeyState.PartnerWorkStatus = [WorkStatusOption.PaidEmployment];
        var result = Assert.IsType<ViewResult>(_controller.PartnerWeeklyEarnings());
        Assert.NotNull(result.Model<PartnerWeeklyEarningsViewModel>());
    }

    [Fact]
    public void PartnerWeeklyEarningsPostUnreachableCoverage()
    {
        var model = new PartnerWeeklyEarningsViewModel
        {
            PartnerWeeklyEarnings = (WeeklyEarningsOption)99,
        };

        Assert.Throws<UnreachableException>(() => _controller.PartnerWeeklyEarnings(model));
    }

    [Theory]
    [InlineData(YearlyEarningsOption.AboveThreshold, null, nameof(PartnerController.PartnerBenefits))]
    [InlineData(YearlyEarningsOption.BelowThreshold, null, nameof(PartnerController.PartnerBenefits))]
    [InlineData(YearlyEarningsOption.AboveThreshold, ReturnTo.CheckAnswers, nameof(PartnerController.PartnerBenefits))]
    public void PartnerYearlyEarningsPostSavesStateAndRedirects(YearlyEarningsOption option, string? returnTo, string actionName)
    {
        _journeyState.PartnerBenefits = [PartnerBenefitsOption.CarersAllowance];
        var model = new PartnerYearlyEarningsViewModel
        {
            PartnerYearlyEarnings = option,
            ReturnTo = returnTo
        };
        var result = _controller.PartnerYearlyEarnings(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).SetState(_journeyState);
        Assert.Equal(option, _journeyState.PartnerYearlyEarnings);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
    }

    [Fact]
    public void PartnerYearlyEarningsPostInvalidSelectionReturnsViewWithError()
    {
        var model = new PartnerYearlyEarningsViewModel();
        _controller.ModelState.AddModelError(nameof(model.PartnerYearlyEarnings), "Faked Model Binding Error");
        var result = _controller.PartnerYearlyEarnings(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PartnerYearlyEarnings)));
        _journeySession.DidNotReceive().SetState(_journeyState);
    }

    [Fact]
    public void PartnerYearlyEarningsGetPopulatesModelFromState()
    {
        _journeyState.PartnerYearlyEarnings = YearlyEarningsOption.AboveThreshold;
        var result = Assert.IsType<ViewResult>(_controller.PartnerYearlyEarnings());
        Assert.Equal(YearlyEarningsOption.AboveThreshold, result.Model<PartnerYearlyEarningsViewModel>().PartnerYearlyEarnings);
    }

    [Fact]
    public void PartnerYearlyEarningsReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.PartnerYearlyEarnings());
        Assert.NotNull(result.Model<PartnerYearlyEarningsViewModel>());
    }

    [Theory]
    [InlineData(PartnerChildcareSupportOption.ChildcareVouchers, null, nameof(PartnerController.PartnerChildcareVoucherReceipt))]
    [InlineData(PartnerChildcareSupportOption.ChildcareBursaryOrGrant, null, nameof(SummaryController.CheckAnswers))]
    [InlineData(PartnerChildcareSupportOption.None, null, nameof(SummaryController.CheckAnswers))]
    [InlineData(PartnerChildcareSupportOption.ChildcareVouchers, ReturnTo.CheckAnswers, nameof(PartnerController.PartnerChildcareVoucherReceipt))]
    public void PartnerChildcareSupportPostSavesStateAndRedirects(PartnerChildcareSupportOption option, string? returnTo, string actionName)
    {
        _journeyState.PartnerChildcareVoucherReceipt = ChildcareVoucherReceiptOption.WorkplaceNurseryScheme;
        var model = new PartnerChildcareSupportViewModel
        {
            PartnerChildcareSupport = [option],
            ReturnTo = returnTo
        };
        var result = _controller.PartnerChildcareSupport(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).SetState(_journeyState);
        Assert.Equal([option], _journeyState.PartnerChildcareSupport);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
    }

    [Fact]
    public void PartnerChildcareSupportPostInvalidSelectionReturnsViewWithError()
    {
        var model = new PartnerChildcareSupportViewModel();
        _controller.ModelState.AddModelError(nameof(model.PartnerChildcareSupport), "Faked Model Binding Error");
        var result = _controller.PartnerChildcareSupport(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PartnerChildcareSupport)));
        _journeySession.DidNotReceive().SetState(_journeyState);
    }

    [Fact]
    public void PartnerChildcareSupportGetPopulatesModelFromState()
    {
        _journeyState.PartnerChildcareSupport = [PartnerChildcareSupportOption.ChildcareVouchers];
        var result = Assert.IsType<ViewResult>(_controller.PartnerChildcareSupport());
        Assert.Equal([PartnerChildcareSupportOption.ChildcareVouchers], result.Model<PartnerChildcareSupportViewModel>().PartnerChildcareSupport);
    }

    [Fact]
    public void PartnerChildcareSupportReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.PartnerChildcareSupport());
        Assert.NotNull(result.Model<PartnerChildcareSupportViewModel>());
    }

    [Theory]
    [InlineData(ChildcareVoucherReceiptOption.WorkplaceNurseryScheme, null, "Summary", nameof(SummaryController.CheckAnswers))]
    [InlineData(ChildcareVoucherReceiptOption.EmployerArrangesWithProvider, null, "Summary", nameof(SummaryController.CheckAnswers))]
    [InlineData(ChildcareVoucherReceiptOption.ThroughSalarySacrifice, null, "Summary", nameof(SummaryController.CheckAnswers))]
    [InlineData(ChildcareVoucherReceiptOption.WorkplaceNurseryScheme, ReturnTo.CheckAnswers, "Summary", nameof(SummaryController.CheckAnswers))]
    public void PartnerChildcareVoucherReceiptPostSavesStateAndRedirects(ChildcareVoucherReceiptOption option, string? returnTo, string controllerName, string actionName)
    {
        var model = new PartnerChildcareVoucherReceiptViewModel
        {
            PartnerChildcareVoucherReceipt = option,
            ReturnTo = returnTo
        };
        var result = _controller.PartnerChildcareVoucherReceipt(model);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).SetState(_journeyState);
        Assert.Equal(option, _journeyState.PartnerChildcareVoucherReceipt);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal(controllerName, redirect.ControllerName);
    }

    [Fact]
    public void PartnerChildcareVoucherReceiptPostInvalidSelectionReturnsViewWithError()
    {
        var model = new PartnerChildcareVoucherReceiptViewModel();
        _controller.ModelState.AddModelError(nameof(model.PartnerChildcareVoucherReceipt), "Faked Model Binding Error");
        var result = _controller.PartnerChildcareVoucherReceipt(model);
        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.PartnerChildcareVoucherReceipt)));
        _journeySession.DidNotReceive().SetState(_journeyState);
    }

    [Fact]
    public void PartnerChildcareVoucherReceiptGetPopulatesModelFromState()
    {
        _journeyState.PartnerChildcareVoucherReceipt = ChildcareVoucherReceiptOption.WorkplaceNurseryScheme;
        var result = Assert.IsType<ViewResult>(_controller.PartnerChildcareVoucherReceipt());
        Assert.Equal(ChildcareVoucherReceiptOption.WorkplaceNurseryScheme, result.Model<PartnerChildcareVoucherReceiptViewModel>().PartnerChildcareVoucherReceipt);
    }

    [Fact]
    public void PartnerChildcareVoucherReceiptReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.PartnerChildcareVoucherReceipt());
        Assert.NotNull(result.Model<PartnerChildcareVoucherReceiptViewModel>());
    }

    public void Dispose() { _controller?.Dispose(); GC.SuppressFinalize(this); }
}

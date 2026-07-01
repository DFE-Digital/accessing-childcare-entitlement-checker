using AccessingChildcareEntitlementChecker.Web.Extensions;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Models.ExpectedChildDetails;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Models.Summary;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class SummaryController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly IStringLocalizerFactory _stringLocalizerFactory;

    public const string Name = "Summary";

    public SummaryController(
        JourneyState journeyState,
        IJourneySession journeySession,
        IStringLocalizerFactory stringLocalizerFactory)
    {
        _journeyState = journeyState;
        _journeySession = journeySession;
        _stringLocalizerFactory = stringLocalizerFactory;
    }

    [HttpGet]
    public ViewResult CheckChildDetails(string? childId = null)
    {
        var summaries = _journeyState.Children.Values.Select(child => ChildSummaryViewModelFactory(child, ReturnTo.CheckChildDetails)).ToList().AsReadOnly();
        var hasChildren = _journeyState.Children.Count > 0;
        var lastEditedChild = ResolveLastEditedChild(_journeyState, childId);
        var backLink = GetCheckChildDetailsBackLink(lastEditedChild);
        return View(new CheckChildDetailsViewModel(summaries, hasChildren, lastEditedChild, backLink));
    }

    [HttpGet]
    public IActionResult CheckAnswers(string? fromChildId = null)
    {
        var summaries = _journeyState.Children.Values.Select(child => ChildSummaryViewModelFactory(child, ReturnTo.CheckAnswers)).ToList().AsReadOnly();
        var hasChildren = _journeyState.Children.Count > 0;
        var lastEditedChild = ResolveLastEditedChild(_journeyState, fromChildId);
        var state = _journeyState;

        var homeBuilder = new SummaryRowFactory(MetadataProvider, "Home", _stringLocalizerFactory)
            .AddLocation(_journeyState.CountryOfResidence);

        var userBuilder = new SummaryRowFactory(MetadataProvider, "User", _stringLocalizerFactory)
            .AddUserAge(state.UserAge)
            .Add((NationalityViewModel m) => m.Nationality, state.Nationality, nameof(UserController.Nationality))
            .Add((SettledStatusViewModel m) => m.SettledStatus, state.SettledStatus, nameof(UserController.SettledStatus))
            .Add((PaidWorkViewModel m) => m.PaidWork, state.PaidWork, nameof(UserController.PaidWork))
            .AddParentalLeave(state)
            .Add((WorkStatusViewModel m) => m.WorkStatus, state.WorkStatus, nameof(UserController.WorkStatus))
            .Add((SelfEmployedDurationViewModel m) => m.SelfEmployedDuration, state.SelfEmployedDuration, nameof(UserController.SelfEmployedDuration))
            .AddWeeklyEarnings(state)
            .Add((YearlyEarningsViewModel m) => m.YearlyEarnings, state.YearlyEarnings, nameof(UserController.YearlyEarnings))
            .Add((UniversalCreditViewModel m) => m.UniversalCredit, state.UniversalCredit, nameof(UserController.UniversalCredit))
            .Add((BenefitsViewModel m) => m.Benefits, state.Benefits, nameof(UserController.Benefits))
            .Add((ChildcareSupportViewModel m) => m.ChildcareSupport, state.ChildcareSupport, nameof(UserController.ChildcareSupport))
            .Add((ChildcareVoucherReceiptViewModel m) => m.ChildcareVoucherReceipt, state.ChildcareVoucherReceipt, nameof(UserController.ChildcareVoucherReceipt))
            .AddHasPartner(state.HasPartner);

        var partnerBuilder = new SummaryRowFactory(MetadataProvider, "Partner", _stringLocalizerFactory)
            .AddPartnerAge(state.PartnerAge)
            .Add((PartnerNationalityViewModel m) => m.PartnerNationality, state.PartnerNationality, nameof(PartnerController.PartnerNationality))
            .Add((PartnerSettledStatusViewModel m) => m.PartnerSettledStatus, state.PartnerSettledStatus, nameof(PartnerController.PartnerSettledStatus))
            .Add((PartnerPaidWorkViewModel m) => m.PartnerPaidWork, state.PartnerPaidWork, nameof(PartnerController.PartnerPaidWork))
            .AddPartnerParentalLeave(state)
            .Add((PartnerWorkStatusViewModel m) => m.PartnerWorkStatus, state.PartnerWorkStatus, nameof(PartnerController.PartnerWorkStatus))
            .Add((PartnerSelfEmployedDurationViewModel m) => m.PartnerSelfEmployedDuration, state.PartnerSelfEmployedDuration, nameof(PartnerController.PartnerSelfEmployedDuration))
            .AddPartnerWeeklyEarnings(state)
            .Add((PartnerYearlyEarningsViewModel m) => m.PartnerYearlyEarnings, state.PartnerYearlyEarnings, nameof(PartnerController.PartnerYearlyEarnings))
            .Add((PartnerBenefitsViewModel m) => m.PartnerBenefits, state.PartnerBenefits, nameof(PartnerController.PartnerBenefits))
            .Add((PartnerChildcareSupportViewModel m) => m.PartnerChildcareSupport, state.PartnerChildcareSupport, nameof(PartnerController.PartnerChildcareSupport))
            .Add((PartnerChildcareVoucherReceiptViewModel m) => m.PartnerChildcareVoucherReceipt, state.PartnerChildcareVoucherReceipt, nameof(PartnerController.PartnerChildcareVoucherReceipt));

        var userDetails = homeBuilder.ViewModels.Concat(userBuilder.ViewModels).ToList().AsReadOnly();
        var partnerDetails = partnerBuilder.ViewModels;
        var backLink = GetCheckAnswersBackLink();
        return View(new CheckAnswersViewModel(summaries, hasChildren, lastEditedChild, userDetails, partnerDetails, backLink));
    }

    [HttpGet]
    public IActionResult Remove(string? childId, string returnTo = ReturnTo.CheckChildDetails)
    {
        if (childId is null || !_journeyState.Children.TryGetValue(childId, out var child))
        {
            return this.RedirectToReturnTo(returnTo);
        }

        return View(new RemoveChildViewModel
        {
            ChildId = childId,
            Name = child.Name,
            ReturnTo = returnTo
        });
    }

    [HttpPost]
    public IActionResult Remove(RemoveChildViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (model.RemoveConfirmed != true)
        {
            return this.RedirectToReturnTo(model.ReturnTo);
        }

        if (_journeyState.Children.Remove(model.ChildId, out var child))
        {
            TempData["RemovedChildName"] = child.Name;
            _journeySession.Set(_journeyState);
        }

        return this.RedirectToReturnTo(model.ReturnTo);
    }

    private ChildSummaryViewModel ChildSummaryViewModelFactory(Child child, string returnTo)
    {
        var born = new SummaryRowFactory(MetadataProvider, "BornChildDetails", _stringLocalizerFactory)
            .Add((ChildBirthDateViewModel m) => m.ChildBirthDate, child.BirthDate, nameof(BornChildDetailsController.ChildBirthDate))
            .Add((ChildSupportViewModel m) => m.ChildSupportOptions, child.ChildSupportOptions, nameof(BornChildDetailsController.ChildSupport));

        var expected = new SummaryRowFactory(MetadataProvider, "ExpectedChildDetails", _stringLocalizerFactory)
            .Add((ChildDueDateViewModel m) => m.ChildDueDate, child.DueDate, nameof(ExpectedChildDetailsController.ChildDueDate));

        var summaryRows = born.ViewModels.Concat(expected.ViewModels).ToList().AsReadOnly();
        return new ChildSummaryViewModel(child.ChildId, child.Name, returnTo, summaryRows);
    }

    private static Child? ResolveLastEditedChild(JourneyState journeyState, string? childId)
    {
        if (childId is not null && journeyState.Children.TryGetValue(childId, out var child))
        {
            return child;
        }

        return journeyState.Children.Values.LastOrDefault();
    }

    private string GetCheckChildDetailsBackLink(Child? child)
    {
        if (child?.BirthStatus == BirthStatus.Born)
        {
            return this.Url.ActionOrThrow(nameof(BornChildDetailsController.ChildSupport), BornChildDetailsController.Name, new { childId = child.ChildId });
        }
        else if (child?.BirthStatus == BirthStatus.Due)
        {
            return this.Url.ActionOrThrow(nameof(ExpectedChildDetailsController.ChildDueDate), ExpectedChildDetailsController.Name, new { childId = child.ChildId });
        }

        return this.Url.ActionOrThrow(nameof(IntroductionController.ChildName), IntroductionController.Name);
    }

    /// <remarks>
    /// Note null forgiving - although not encoded in the types we expect all required questions
    /// to have values at this point; and fail fast if not!
    /// </remarks>
    private string GetCheckAnswersBackLink()
    {
        if (_journeyState.HasPartner!.Value)
        {
            if (_journeyState.PartnerChildcareSupport.Contains(PartnerChildcareSupportOption.ChildcareVouchers))
            {
                return this.Url.ActionOrThrow(nameof(PartnerController.PartnerChildcareVoucherReceipt), PartnerController.Name);

            }
            else
            {
                return this.Url.ActionOrThrow(nameof(PartnerController.PartnerChildcareSupport), PartnerController.Name);
            }
        }

        return this.Url.ActionOrThrow(nameof(UserController.HasPartner), UserController.Name);
    }
}

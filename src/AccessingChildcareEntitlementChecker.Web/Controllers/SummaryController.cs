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
using static AccessingChildcareEntitlementChecker.Web.IServiceCollectionExtensions;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class SummaryController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly IStringLocalizerFactory _stringLocalizerFactory;
    private readonly Journey _journey;

    public SummaryController(
        JourneyState journeyState,
        IJourneySession journeySession,
        IStringLocalizerFactory stringLocalizerFactory,
        Journey journey)
    {
        _journeyState = journeyState;
        _journeySession = journeySession;
        _stringLocalizerFactory = stringLocalizerFactory;
        _journey = journey;
    }

    [HttpGet]
    public ViewResult CheckChildDetails(string? fromChildId = null)
    {
        var summaries = _journeyState.Children.Values.Select(child => ChildSummaryViewModelFactory(child, ReturnTo.CheckChildDetails)).ToList().AsReadOnly();
        var hasChildren = _journeyState.Children.Count > 0;
        var lastEditedChild = ResolveLastEditedChild(_journeyState, fromChildId);
        ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState);
        return View(new CheckChildDetailsViewModel(summaries, hasChildren, lastEditedChild));
    }

    [HttpGet]
    public IActionResult CheckAnswers(string? fromChildId = null)
    {
        var summaries = _journeyState.Children.Values.Select(child => ChildSummaryViewModelFactory(child, ReturnTo.CheckAnswers)).ToList().AsReadOnly();
        var hasChildren = _journeyState.Children.Count > 0;
        var lastEditedChild = ResolveLastEditedChild(_journeyState, fromChildId);
        var state = _journeyState;
        ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState);

        var homeBuilder = new SummaryRowFactory(MetadataProvider, "Home", _stringLocalizerFactory)
            .AddLocation(_journeyState.CountryOfResidence);

        var userBuilder = new SummaryRowFactory(MetadataProvider, "User", _stringLocalizerFactory)
            .AddUserAge(state.UserAge)
            .Add((NationalityViewModel m) => m.Nationality, state.Nationality, nameof(UserController.Nationality))
            .Add((SettledStatusViewModel m) => m.SettledStatus, state.SettledStatus, nameof(UserController.SettledStatus))
            .Add((PaidWorkViewModel m) => m.PaidWork, state.PaidWork, nameof(UserController.PaidWork))
            .Add((WorkStatusViewModel m) => m.WorkStatus, state.WorkStatus, nameof(UserController.WorkStatus))
            .Add((SelfEmployedDurationViewModel m) => m.SelfEmployedDuration, state.SelfEmployedDuration, nameof(UserController.SelfEmployedDuration))
            .Add((WeeklyEarningsViewModel m) => m.WeeklyEarnings, state.WeeklyEarnings, nameof(UserController.WeeklyEarnings))
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
            .Add((PartnerWorkStatusViewModel m) => m.PartnerWorkStatus, state.PartnerWorkStatus, nameof(PartnerController.PartnerWorkStatus))
            .Add((PartnerSelfEmployedDurationViewModel m) => m.PartnerSelfEmployedDuration, state.PartnerSelfEmployedDuration, nameof(PartnerController.PartnerSelfEmployedDuration))
            .Add((PartnerWeeklyEarningsViewModel m) => m.PartnerWeeklyEarnings, state.PartnerWeeklyEarnings, nameof(PartnerController.PartnerWeeklyEarnings))
            .Add((PartnerYearlyEarningsViewModel m) => m.PartnerYearlyEarnings, state.PartnerYearlyEarnings, nameof(PartnerController.PartnerYearlyEarnings))
            .Add((PartnerBenefitsViewModel m) => m.PartnerBenefits, state.PartnerBenefits, nameof(PartnerController.PartnerBenefits))
            .Add((PartnerChildcareSupportViewModel m) => m.PartnerChildcareSupport, state.PartnerChildcareSupport, nameof(PartnerController.PartnerChildcareSupport))
            .Add((PartnerChildcareVoucherReceiptViewModel m) => m.PartnerChildcareVoucherReceipt, state.PartnerChildcareVoucherReceipt, nameof(PartnerController.PartnerChildcareVoucherReceipt));

        var userDetails = homeBuilder.ViewModels.Concat(userBuilder.ViewModels).ToList().AsReadOnly();
        var partnerDetails = partnerBuilder.ViewModels;

        return View(new CheckAnswersViewModel(summaries, hasChildren, lastEditedChild, userDetails, partnerDetails));
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
            .Add((ChildRelationshipViewModel m) => m.Relationship, child.BornRelationship, nameof(BornChildDetailsController.ChildRelationship))
            .Add((ChildSupportViewModel m) => m.ChildSupportOptions, child.ChildSupportOptions, nameof(BornChildDetailsController.ChildSupport));

        var expected = new SummaryRowFactory(MetadataProvider, "ExpectedChildDetails", _stringLocalizerFactory)
            .Add((ChildDueDateViewModel m) => m.ChildDueDate, child.DueDate, nameof(ExpectedChildDetailsController.ChildDueDate))
            .Add((ExpectedChildRelationshipViewModel m) => m.ExpectedChildRelationship, child.ExpectedRelationship, nameof(ExpectedChildDetailsController.ExpectedChildRelationship));

        var summaryRows = born.ViewModels.Concat(expected.ViewModels).ToList().AsReadOnly();
        return new ChildSummaryViewModel(child.ChildId, child.Name, returnTo, summaryRows);
    }

    private static Child? ResolveLastEditedChild(JourneyState journeyState, string? fromChildId)
    {

        if (fromChildId is not null && journeyState.Children.TryGetValue(fromChildId, out var fromChild))
        {
            return fromChild;
        }

        return journeyState.Children.Values.LastOrDefault();
    }
}

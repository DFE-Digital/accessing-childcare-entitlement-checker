using AccessingChildcareEntitlementChecker.Web.Extensions;
using AccessingChildcareEntitlementChecker.Web.Filters;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Models.ExpectedChildDetails;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Models.Summary;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

[ServiceFilter(typeof(RequireJourneySessionFilter))]
public partial class SummaryController(
    JourneyState journeyState,
    IJourneySession journeySession,
    IStringLocalizerFactory stringLocalizerFactory,
    ILogger<SummaryController> logger)
    : Controller
{
    public const string Name = "Summary";

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Warning,
        Message = "State mismatch detected. Correlation ID mismatch.")]
    private partial void LogCorrelationIdMismatch();

    [HttpGet]
    public ViewResult CheckChildDetails(string? childId = null)
    {
        var summaries = journeyState.Children.Values.Select(child => ChildSummaryViewModelFactory(child, ReturnTo.CheckChildDetails)).ToList().AsReadOnly();
        var hasChildren = journeyState.Children.Count > 0;
        var lastEditedChild = ResolveLastEditedChild(journeyState, childId);
        var backLink = GetCheckChildDetailsBackLink(lastEditedChild);
        return View(new CheckChildDetailsViewModel(summaries, hasChildren, lastEditedChild, backLink, journeyState.CorrelationId));
    }

    [HttpPost]
    public IActionResult CheckChildDetails(CheckChildDetailsSubmitModel model)
    {
        // NOTE: Returning a custom 400 StateMismatch page on mismatch is a suggested fallback.
        if (model.CorrelationId != journeyState.CorrelationId)
        {
            journeySession.Clear();
            LogCorrelationIdMismatch();
            Response.StatusCode = 400;
            return View("StateMismatch");
        }

        return RedirectToAction(nameof(UserController.UserAge), UserController.Name);
    }

    [HttpGet]
    public IActionResult CheckAnswers(string? fromChildId = null)
    {
        var summaries = journeyState.Children.Values.Select(child => ChildSummaryViewModelFactory(child, ReturnTo.CheckAnswers)).ToList().AsReadOnly();
        var hasChildren = journeyState.Children.Count > 0;
        var lastEditedChild = ResolveLastEditedChild(journeyState, fromChildId);

        var homeBuilder = new SummaryRowFactory(MetadataProvider, "Home", stringLocalizerFactory)
            .AddLocation(journeyState.CountryOfResidence);

        var userBuilder = new SummaryRowFactory(MetadataProvider, "User", stringLocalizerFactory)
            .AddUserAge(journeyState.UserAge)
            .Add((NationalityViewModel m) => m.Nationality, journeyState.Nationality, nameof(UserController.Nationality))
            .Add((SettledStatusViewModel m) => m.SettledStatus, journeyState.SettledStatus, nameof(UserController.SettledStatus))
            .Add((PaidWorkViewModel m) => m.PaidWork, journeyState.PaidWork, nameof(UserController.PaidWork))
            .AddParentalLeave(journeyState)
            .Add((WorkStatusViewModel m) => m.WorkStatus, journeyState.WorkStatus, nameof(UserController.WorkStatus))
            .Add((SelfEmployedDurationViewModel m) => m.SelfEmployedDuration, journeyState.SelfEmployedDuration, nameof(UserController.SelfEmployedDuration))
            .AddWeeklyEarnings(journeyState)
            .Add((YearlyEarningsViewModel m) => m.YearlyEarnings, journeyState.YearlyEarnings, nameof(UserController.YearlyEarnings))
            .Add((UniversalCreditViewModel m) => m.UniversalCredit, journeyState.UniversalCredit, nameof(UserController.UniversalCredit))
            .Add((BenefitsViewModel m) => m.Benefits, journeyState.Benefits, nameof(UserController.Benefits))
            .Add((ChildcareSupportViewModel m) => m.ChildcareSupport, journeyState.ChildcareSupport, nameof(UserController.ChildcareSupport))
            .Add((ChildcareVoucherReceiptViewModel m) => m.ChildcareVoucherReceipt, journeyState.ChildcareVoucherReceipt, nameof(UserController.ChildcareVoucherReceipt))
            .AddHasPartner(journeyState.HasPartner);

        var partnerBuilder = new SummaryRowFactory(MetadataProvider, "Partner", stringLocalizerFactory)
            .AddPartnerAge(journeyState.PartnerAge)
            .Add((PartnerNationalityViewModel m) => m.PartnerNationality, journeyState.PartnerNationality, nameof(PartnerController.PartnerNationality))
            .Add((PartnerSettledStatusViewModel m) => m.PartnerSettledStatus, journeyState.PartnerSettledStatus, nameof(PartnerController.PartnerSettledStatus))
            .Add((PartnerPaidWorkViewModel m) => m.PartnerPaidWork, journeyState.PartnerPaidWork, nameof(PartnerController.PartnerPaidWork))
            .AddPartnerParentalLeave(journeyState)
            .Add((PartnerWorkStatusViewModel m) => m.PartnerWorkStatus, journeyState.PartnerWorkStatus, nameof(PartnerController.PartnerWorkStatus))
            .Add((PartnerSelfEmployedDurationViewModel m) => m.PartnerSelfEmployedDuration, journeyState.PartnerSelfEmployedDuration, nameof(PartnerController.PartnerSelfEmployedDuration))
            .AddPartnerWeeklyEarnings(journeyState)
            .Add((PartnerYearlyEarningsViewModel m) => m.PartnerYearlyEarnings, journeyState.PartnerYearlyEarnings, nameof(PartnerController.PartnerYearlyEarnings))
            .Add((PartnerBenefitsViewModel m) => m.PartnerBenefits, journeyState.PartnerBenefits, nameof(PartnerController.PartnerBenefits))
            .Add((PartnerChildcareSupportViewModel m) => m.PartnerChildcareSupport, journeyState.PartnerChildcareSupport, nameof(PartnerController.PartnerChildcareSupport))
            .Add((PartnerChildcareVoucherReceiptViewModel m) => m.PartnerChildcareVoucherReceipt, journeyState.PartnerChildcareVoucherReceipt, nameof(PartnerController.PartnerChildcareVoucherReceipt));

        var userDetails = homeBuilder.ViewModels.Concat(userBuilder.ViewModels).ToList().AsReadOnly();
        var partnerDetails = partnerBuilder.ViewModels;
        var backLink = GetCheckAnswersBackLink();
        return View(new CheckAnswersViewModel(summaries, hasChildren, lastEditedChild, userDetails, partnerDetails, backLink, journeyState.CorrelationId));
    }

    [HttpPost]
    public IActionResult CheckAnswers(CheckAnswersSubmitModel model)
    {
        // NOTE: Returning a custom 400 StateMismatch page on mismatch is a suggested fallback.
        if (model.CorrelationId != journeyState.CorrelationId)
        {
            journeySession.Clear();
            LogCorrelationIdMismatch();
            Response.StatusCode = 400;
            return View("StateMismatch");
        }

        return RedirectToAction(nameof(ResultsController.Results), ResultsController.Name);
    }

    [HttpGet]
    public IActionResult Remove(string? childId, string returnTo = ReturnTo.CheckChildDetails)
    {
        if (childId is null || !journeyState.Children.TryGetValue(childId, out var child))
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

        if (journeyState.Children.Remove(model.ChildId, out var child))
        {
            TempData["RemovedChildName"] = child.Name;
            journeySession.Set(journeyState);
        }

        return this.RedirectToReturnTo(model.ReturnTo);
    }

    private ChildSummaryViewModel ChildSummaryViewModelFactory(Child child, string returnTo)
    {
        var born = new SummaryRowFactory(MetadataProvider, "BornChildDetails", stringLocalizerFactory)
            .Add((ChildBirthDateViewModel m) => m.ChildBirthDate, child.BirthDate, nameof(BornChildDetailsController.ChildBirthDate))
            .Add((ChildSupportViewModel m) => m.ChildSupportOptions, child.ChildSupportOptions, nameof(BornChildDetailsController.ChildSupport));

        var expected = new SummaryRowFactory(MetadataProvider, "ExpectedChildDetails", stringLocalizerFactory)
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
            return Url.ActionOrThrow(nameof(BornChildDetailsController.ChildSupport), BornChildDetailsController.Name, new { childId = child.ChildId });
        }

        if (child?.BirthStatus == BirthStatus.Due)
        {
            return Url.ActionOrThrow(nameof(ExpectedChildDetailsController.ChildDueDate), ExpectedChildDetailsController.Name, new { childId = child.ChildId });
        }

        return Url.ActionOrThrow(nameof(IntroductionController.ChildName), IntroductionController.Name);
    }

    /// <remarks>
    /// Note null forgiving - although not encoded in the types we expect all required questions
    /// to have values at this point; and fail fast if not!
    /// </remarks>
    private string GetCheckAnswersBackLink()
    {
        if (journeyState.HasPartner!.Value)
        {
            if (journeyState.PartnerChildcareSupport.Contains(PartnerChildcareSupportOption.ChildcareVouchers))
            {
                return Url.ActionOrThrow(nameof(PartnerController.PartnerChildcareVoucherReceipt), PartnerController.Name);

            }

            return Url.ActionOrThrow(nameof(PartnerController.PartnerChildcareSupport), PartnerController.Name);
        }

        return Url.ActionOrThrow(nameof(UserController.HasPartner), UserController.Name);
    }
}

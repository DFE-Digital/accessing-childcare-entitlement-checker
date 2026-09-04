using Dfe.Acec.Web.Extensions;
using Dfe.Acec.Web.Filters;
using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.BornChildDetails;
using Dfe.Acec.Web.Models.ExpectedChildDetails;
using Dfe.Acec.Web.Models.Partner;
using Dfe.Acec.Web.Models.Summary;
using Dfe.Acec.Web.Models.User;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.FeatureManagement;

namespace Dfe.Acec.Web.Controllers;

[ServiceFilter(typeof(RequireJourneySessionFilter))]
public partial class SummaryController(
    JourneyState journeyState,
    IJourneySession journeySession,
    IStringLocalizerFactory stringLocalizerFactory,
    IValidator<JourneyState> journeyStateValidator,
    ILogger<SummaryController> logger,
    IFeatureManager featureManager)
    : Controller
{
    public const string Name = "Summary";
    private const string StateMismatchView = "StateMismatch";

    [HttpGet]
    public ViewResult CheckChildDetails(string? childId = null)
    {
        var removedChildNames = CheckForIncompleteChildren();

        return View(BuildCheckChildDetailsViewModel(childId, removedChildNames));
    }

    [HttpPost]
    public IActionResult CheckChildDetails(CheckChildDetailsSubmitModel model)
    {
        if (model.CorrelationId == journeyState.CorrelationId)
        {
            var result = journeyStateValidator.Validate(journeyState, options => options.IncludeRuleSets(JourneyStateValidator.CheckChildDetailsRuleSet));
            if (result.IsValid)
            {
                return RedirectToAction(nameof(UserController.UserAge), UserController.Name);
            }

            ModelState.AddValidationErrors(result);
            return View(BuildCheckChildDetailsViewModel());
        }

        LogCorrelationIdMismatch();
        Response.StatusCode = 400;
        return View(StateMismatchView);
    }

    [HttpGet]
    public async Task<IActionResult> CheckAnswers(string? fromChildId = null)
    {
        var removedChildNames = CheckForIncompleteChildren();

        return View(await BuildCheckAnswersViewModel(fromChildId, removedChildNames));
    }

    [HttpPost]
    public IActionResult CheckAnswers(CheckAnswersSubmitModel model)
    {
        if (model.CorrelationId == journeyState.CorrelationId)
        {
            var result = journeyStateValidator.Validate(journeyState, options => options.IncludeRuleSets(JourneyStateValidator.CheckAnswersRuleSet));
            if (result.IsValid)
            {
                return RedirectToAction(nameof(ResultsController.Results), ResultsController.Name);
            }

            LogMissingAnswers();
            Response.StatusCode = 400;
            return View(StateMismatchView);
        }

        LogCorrelationIdMismatch();
        Response.StatusCode = 400;
        return View(StateMismatchView);
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
            journeySession.SetState(journeyState);
        }

        return this.RedirectToReturnTo(model.ReturnTo);
    }

    private List<string> CheckForIncompleteChildren()
    {
        var removedChildNames = new List<string>();

        var result = journeyStateValidator.Validate(journeyState, options => options.IncludeRuleSets(JourneyStateValidator.CheckChildDetailsRuleSet));

        if (!result.IsValid)
        {
            var invalidChildIds = result.Errors
                .Select(error => error.CustomState)
                .OfType<string>()
                .Distinct()
                .ToList();

            var invalidChildren = journeyState.Children
                .Where(x => invalidChildIds.Contains(x.Key))
                .ToList();

            removedChildNames = [.. invalidChildren.Select(x => x.Value.Name)];

            foreach (var child in invalidChildren)
            {
                journeyState.Children.Remove(child.Key);
            }

            journeySession.SetState(journeyState);
        }

        return removedChildNames;
    }

    private CheckChildDetailsViewModel BuildCheckChildDetailsViewModel(string? childId = null, IReadOnlyList<string>? removedChildNames = null)
    {
        var summaries = journeyState.Children.Values.Select(child => ChildSummaryViewModelFactory(child, ReturnTo.CheckChildDetails)).ToList().AsReadOnly();
        var hasChildren = journeyState.Children.Count > 0;
        var lastEditedChild = ResolveLastEditedChild(journeyState, childId);
        var backLink = GetCheckChildDetailsBackLink(lastEditedChild);
        return new CheckChildDetailsViewModel(summaries, hasChildren, lastEditedChild, backLink, journeyState.CorrelationId, removedChildNames ?? []);
    }

    private async Task<CheckAnswersViewModel> BuildCheckAnswersViewModel(string? fromChildId = null, IReadOnlyList<string>? removedChildNames = null)
    {
        var summaries = journeyState.Children.Values.Select(child => ChildSummaryViewModelFactory(child, ReturnTo.CheckAnswers)).ToList().AsReadOnly();
        var hasChildren = journeyState.Children.Count > 0;
        var lastEditedChild = ResolveLastEditedChild(journeyState, fromChildId);

        var homeBuilder = new SummaryRowFactory(
            MetadataProvider,
            "Home",
            stringLocalizerFactory);

        if (!await featureManager.IsEnabledAsync(FeatureFlags.HmrcIntegration))
        {
            homeBuilder.AddLocation(journeyState.CountryOfResidence);
        }

        var userBuilder = new SummaryRowFactory(MetadataProvider, "User", stringLocalizerFactory)
            .AddUserAge(journeyState.UserAge)
            .Add((NationalityViewModel m) => m.NationalityOptions, journeyState.NationalityOptions, nameof(UserController.Nationality))
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
            .Add((PartnerNationalityViewModel m) => m.PartnerNationalityOptions, journeyState.PartnerNationalityOptions, nameof(PartnerController.PartnerNationality))
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
        return new CheckAnswersViewModel(summaries, hasChildren, lastEditedChild, userDetails, partnerDetails, backLink, journeyState.CorrelationId, removedChildNames ?? []);
    }

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Warning,
        Message = "State mismatch detected. Correlation ID mismatch. Event: {microsoft.custom_event.name}")]
    private partial void LogCorrelationIdMismatch([TagName("microsoft.custom_event.name")] string customEventName = "StateMismatch");

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Warning,
        Message = "Missing answers detected. Missing answers. Event: {microsoft.custom_event.name}")]
    private partial void LogMissingAnswers([TagName("microsoft.custom_event.name")] string customEventName = "MissingAnswers");

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

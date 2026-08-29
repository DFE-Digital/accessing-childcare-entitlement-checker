using Dfe.Acec.Web.Extensions;
using Dfe.Acec.Web.Filters;
using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.Summary;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Services.Summary;
using Dfe.Acec.Web.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Acec.Web.Controllers;

[ServiceFilter(typeof(RequireJourneySessionFilter))]
public partial class SummaryController(
    JourneyState journeyState,
    IJourneySession journeySession,
    IValidator<JourneyState> journeyStateValidator,
    ILogger<SummaryController> logger,
    ISummaryViewModelBuilder summaryViewModelBuilder)
    : Controller
{
    public const string Name = "Summary";
    private const string StateMismatchView = "StateMismatch";

    [HttpGet]
    public ViewResult CheckChildDetails(string? childId = null)
    {
        var removedChildNames = CheckForIncompleteChildren();

        return View(summaryViewModelBuilder.BuildCheckChildDetailsViewModel(journeyState, Url, childId, removedChildNames));
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
            return View(summaryViewModelBuilder.BuildCheckChildDetailsViewModel(journeyState, Url));
        }

        LogCorrelationIdMismatch();
        Response.StatusCode = 400;
        return View(StateMismatchView);
    }

    [HttpGet]
    public async Task<IActionResult> CheckAnswers(string? fromChildId = null)
    {
        var removedChildNames = CheckForIncompleteChildren();

        return View(await summaryViewModelBuilder.BuildCheckAnswersViewModelAsync(journeyState, Url, fromChildId, removedChildNames));
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

            removedChildNames = invalidChildren
                .Select(x => x.Value.Name)
                .ToList();

            foreach (var child in invalidChildren)
            {
                journeyState.Children.Remove(child.Key);
            }

            journeySession.SetState(journeyState);
        }

        return removedChildNames;
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
}

using Dfe.Acec.RulesEngine.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using Dfe.Acec.Web.Filters;
using Dfe.Acec.Web.Mappers;
using Dfe.Acec.Web.Services;

namespace Dfe.Acec.Web.Controllers;

[ExcludeFromCodeCoverage]
[ServiceFilter(typeof(RequireJourneySessionFilter))]
public class ResultsController(
    JourneyState journeyState,
    JourneyStateToEntitlementRequestMapper journeyStateMapper,
    EntitlementResponseToResultsSummaryViewModelMapper resultsSummaryMapper,
    EntitlementResponseToResultsDetailsViewModelMapper resultsDetailsModelMapper,
    EntitlementRulesEngine rulesEngine) : Controller
{

    private readonly JourneyState _journeyState = journeyState;
    private readonly JourneyStateToEntitlementRequestMapper _journeyStateMapper = journeyStateMapper;
    private readonly EntitlementResponseToResultsSummaryViewModelMapper _resultsSummaryMapper = resultsSummaryMapper;
    private readonly EntitlementResponseToResultsDetailsViewModelMapper _resultsDetailsModelMapper = resultsDetailsModelMapper;
    private readonly EntitlementRulesEngine _rulesEngine = rulesEngine;

    public const string Name = "Results";

    [HttpGet]
    public IActionResult Results()
    {
        var request = _journeyStateMapper.Map(_journeyState);

        var response = _rulesEngine.Evaluate(request, DateOnly.FromDateTime(DateTime.Today));

        return response.ChildResults.SelectMany(c => c.Schemes).Any()
            ? View(_resultsSummaryMapper.Map(response))
            : View("ResultsNotEligible");
    }

    [HttpGet]
    public IActionResult ResultsDetailed(string childId)
    {
        var request = _journeyStateMapper.Map(_journeyState);
        var response = _rulesEngine.Evaluate(request, DateOnly.FromDateTime(DateTime.Today));
        var child = response.ChildResults.SingleOrDefault(x => x.ChildId == childId);

        if (child is null)
        {
            return BadRequest();
        }

        var resultsDetailsViewModel = _resultsDetailsModelMapper.Map(child, response.HasAccessToPublicFunds);
        return View(resultsDetailsViewModel);
    }
}

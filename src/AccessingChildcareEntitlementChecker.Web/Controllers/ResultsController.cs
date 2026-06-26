using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using AccessingChildcareEntitlementChecker.RulesEngine.Services;
using AccessingChildcareEntitlementChecker.Web.Mappers;
using AccessingChildcareEntitlementChecker.Web.Models.Results;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.Web.Controllers
{
    [ExcludeFromCodeCoverage]
    public class ResultsController : Controller
    {

        private readonly JourneyState _journeyState;
        private readonly JourneyStateToEntitlementRequestMapper _journeyStateMapper;
        private readonly EntitlementResponseToResultsSummaryViewModelMapper _resultsSummaryMapper;
        private readonly EntitlementResponseToResultsDetailsViewModelMapper _resultsDetailsModelMapper;
        private readonly EntitlementRulesEngine _rulesEngine;

        public const string Name = "Results";

        public ResultsController(
            JourneyState journeyState,
            JourneyStateToEntitlementRequestMapper journeyStateMapper,
            EntitlementResponseToResultsSummaryViewModelMapper resultsSummaryMapper,
            EntitlementResponseToResultsDetailsViewModelMapper resultsDetailsModelMapper,
            EntitlementRulesEngine rulesEngine)
        {
            _journeyState = journeyState;
            _journeyStateMapper = journeyStateMapper;
            _resultsSummaryMapper = resultsSummaryMapper;
            _resultsDetailsModelMapper = resultsDetailsModelMapper;
            _rulesEngine = rulesEngine;
        }


        [HttpGet]
        public IActionResult Results()
        {
            var request = _journeyStateMapper.Map(_journeyState);

            var response = _rulesEngine.Evaluate(request, DateOnly.FromDateTime(DateTime.Today));

            var resultsSummaryViewModel = _resultsSummaryMapper.Map(response);

            return View(resultsSummaryViewModel);
        }

        [HttpGet]
        public IActionResult ResultsDetailed(string childID)
        {
            var request = _journeyStateMapper.Map(_journeyState);

            var response = _rulesEngine.Evaluate(request, DateOnly.FromDateTime(DateTime.Today));

            var resultsDetailsViewModel = _resultsDetailsModelMapper.Map(response, childID);

            return View(resultsDetailsViewModel);
        }
    }
}

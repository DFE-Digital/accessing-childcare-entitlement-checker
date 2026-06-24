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
        private readonly JourneyStateToEntitlementRequestMapper _mapper;
        private readonly EntitlementResponseToResultsViewModelMapper _mapperER;
        private readonly EntitlementRulesEngine _rulesEngine;

        public ResultsController(
            JourneyState journeyState,
            JourneyStateToEntitlementRequestMapper mapper,
            EntitlementResponseToResultsViewModelMapper mapperER,
            EntitlementRulesEngine rulesEngine)
        {
            _journeyState = journeyState;
            _mapper = mapper;
            _mapperER = mapperER;
            _rulesEngine = rulesEngine;
        }


        [HttpGet]
        public IActionResult Results()
        {
            var request = _mapper.Map(_journeyState);

            var response = _rulesEngine.Evaluate(request, DateOnly.FromDateTime(DateTime.Today));

            var resultsViewModel = _mapperER.Map(response);

            return View(resultsViewModel);
        }
    }
}

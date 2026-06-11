using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using AccessingChildcareEntitlementChecker.RulesEngine.Services;
using AccessingChildcareEntitlementChecker.Web.Mappers;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.Web.Controllers
{
    [ExcludeFromCodeCoverage]
    public class ResultsController : Controller
    {

        private readonly JourneyState _journeyState;
        private readonly JourneyStateToEntitlementRequestMapper _mapper;
        private readonly EntitlementRulesEngine _rulesEngine;
        private readonly Journey _journey;

        public ResultsController(
            JourneyState journeyState,
            JourneyStateToEntitlementRequestMapper mapper,
            EntitlementRulesEngine rulesEngine,
            Journey journey)
        {
            _journeyState = journeyState;
            _mapper = mapper;
            _rulesEngine = rulesEngine;
            _journey = journey;
        }


        [HttpGet]
        public IActionResult Results()
        {
            ViewData["BackLinkHref"] = _journey.Backwards(this, _journeyState);
            var request = _mapper.Map(_journeyState);

            var response = _rulesEngine.Evaluate(request, DateOnly.FromDateTime(DateTime.Today));

            return View(response);
        }
    }
}

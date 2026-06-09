using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using AccessingChildcareEntitlementChecker.RulesEngine.Services;
using AccessingChildcareEntitlementChecker.Web.Mappers;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.Web.Controllers
{
    public class ResultsController : Controller
    {

        private readonly JourneyState _journeyState;
        private readonly JourneyStateToEntitlementRequestMapper _mapper;
        private readonly EntitlementRulesEngine _rulesEngine;

        public ResultsController(
            JourneyState journeyState,
            JourneyStateToEntitlementRequestMapper mapper,
            EntitlementRulesEngine rulesEngine)
        {
            _journeyState = journeyState;
            _mapper = mapper;
            _rulesEngine = rulesEngine;
        }


        [HttpGet]
        public IActionResult Results()
        {
            var request = _mapper.Map(_journeyState);

            var response = _rulesEngine.Evaluate(request, DateOnly.FromDateTime(DateTime.Today));

            return View(response);
        }
    }
}

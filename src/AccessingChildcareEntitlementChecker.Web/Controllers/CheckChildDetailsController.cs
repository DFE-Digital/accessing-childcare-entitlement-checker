using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace AccessingChildcareEntitlementChecker.Web.Controllers
{
    public class CheckChildDetailsController : Controller
    {
        private readonly JourneyState _journeyState;

        [ExcludeFromCodeCoverage(Justification = "To be covered by future pages")]
        public CheckChildDetailsController(
            JourneyState journeyState)
        {
            _journeyState = journeyState;
        }

        [HttpGet]
        [ExcludeFromCodeCoverage(Justification = "To be covered by future pages")]
        public ViewResult CheckChildDetails()
        {
            return View(new CheckChildDetailsViewModel(_journeyState));
        }
    }
}

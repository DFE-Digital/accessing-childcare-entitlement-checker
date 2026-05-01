using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers
{
    public class ChildDetailsController : Controller
    {
        private readonly JourneyState _journeyState;
        private readonly IJourneySession _journeySession;

        public ChildDetailsController(JourneyState journeyState, IJourneySession journeySession)
        {
            _journeyState = journeyState;
            _journeySession = journeySession;
        }

        [HttpGet]
        public ViewResult ChildBirthDate()
        {
            return View();
        }

        [HttpGet]
        public ViewResult ChildDueDate()
        {
            return View();
        }
    }
}

using AccessingChildcareEntitlementChecker.Web.Models.ExpectedChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers
{
    public class ExpectedChildDetailsController : Controller
    {
        private readonly JourneyState _journeyState;
        private readonly IJourneySession _journeySession;

        public ExpectedChildDetailsController(
            JourneyState journeyState,
            IJourneySession journeySession)
        {
            _journeyState = journeyState;
            _journeySession = journeySession;
        }

        [HttpGet]
        public ViewResult ChildDueDate()
        {
            return View(new ChildDueDateViewModel(_journeyState));
        }

        [HttpPost]
        public IActionResult ChildDueDate(ChildDueDateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _journeyState.Apply(model);
            _journeySession.Set(_journeyState);
            return RedirectToAction(nameof(ExpectedChildRelationship), "ExpectedChildDetails");
        }

        [HttpGet]
        public ViewResult ExpectedChildRelationship()
        {
            return View(new ExpectedChildRelationshipViewModel(_journeyState));
        }

        [HttpPost]
        public IActionResult ExpectedChildRelationship(ExpectedChildRelationshipViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _journeyState.Apply(model);
            _journeySession.Set(_journeyState);

            return RedirectToAction(nameof(CheckChildDetailsController.CheckChildDetails), "CheckChildDetails");
        }
    }
}

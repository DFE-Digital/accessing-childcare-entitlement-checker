using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers
{
    public class BornChildDetailsController : Controller
    {
        private readonly JourneyState _journeyState;
        private readonly IJourneySession _journeySession;

        public BornChildDetailsController(
            JourneyState journeyState,
            IJourneySession journeySession)
        {
            _journeyState = journeyState;
            _journeySession = journeySession;
        }

        [HttpGet]
        public ViewResult ChildBirthDate()
        {
            return View(new ChildBirthDateViewModel(_journeyState));
        }

        [HttpPost]
        public IActionResult ChildBirthDate(ChildBirthDateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ChildName = _journeyState.ChildName
                    ?? throw new InvalidOperationException($"{nameof(JourneyState.ChildName)} must be available before rendering {nameof(ChildBirthDate)}.");
                return View(model);
            }

            _journeyState.Apply(model);
            _journeySession.Set(_journeyState);
            return RedirectToAction(nameof(ChildRelationship), "BornChildDetails");
        }

        [HttpGet]
        public ViewResult ChildDueDate()
        {
            return View();
        }

        [HttpGet]
        public ViewResult ChildRelationship()
        {
            return View(new ChildRelationshipViewModel(_journeyState));
        }
    }
}

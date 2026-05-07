using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers
{
    public class BornChildDetailsController : Controller
    {
        private readonly JourneyState _journeyState;
        private readonly IJourneySession _journeySession;

        public ChildDetailsController(
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
                model.ChildName = _journeyState.ChildName;
                return View(model);
            }

            _journeyState.Apply(model);
            _journeySession.Set(_journeyState);
            return RedirectToAction(nameof(ChildRelationship), "ChildDetails");
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

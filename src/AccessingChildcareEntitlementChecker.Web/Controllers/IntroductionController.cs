using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers
{
    public class IntroductionController : Controller
    {
        private readonly JourneyState _journeyState;
        private readonly IJourneySession _journeySession;

        public IntroductionController(JourneyState journeyState, IJourneySession journeySession)
        {
            _journeyState = journeyState;
            _journeySession = journeySession;
        }

        [HttpGet]
        public ViewResult ChildName()
        {
            return View(new ChildNameViewModel(_journeyState));
        }

        [HttpPost]
        public IActionResult ChildName(ChildNameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _journeyState.Apply(model);
            _journeySession.Set(_journeyState);

            return RedirectToAction(nameof(IntroductionController.IsChildBorn), "Introduction");
        }

        [HttpGet]
        public ViewResult IsChildBorn()
        {
            return View(new ChildIsBornViewModel(_journeyState));
        }

        [HttpPost]
        public IActionResult IsChildBorn(ChildIsBornViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _journeyState.Apply(model);
            _journeySession.Set(_journeyState);

            if (model.ChildIsBorn == BirthStatus.Born)
            {
                return RedirectToAction(nameof(ChildDetailsController.ChildBirthDate), "ChildDetails");
            }

            return RedirectToAction(nameof(ChildDetailsController.ChildDueDate), "ChildDetails");
        }
    }
}

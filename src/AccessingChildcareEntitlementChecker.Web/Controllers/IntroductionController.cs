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
        public IActionResult ChildName(string? childId = null)
        {
            if (childId == null)
            {
                var childNameViewModel = new ChildNameViewModel();
                return View(childNameViewModel);
            }

            var child = _journeyState.GetChild(childId);
            if (child == null)
            {
                return RedirectToAction(nameof(ErrorController.NotFound), "Error");
            }

            return View(new ChildNameViewModel(child));
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

            return RedirectToAction(nameof(IntroductionController.IsChildBorn), "Introduction",
                new { childId = model.ChildId });
        }

        [HttpGet]
        public IActionResult IsChildBorn(string childId, string? returnTo = null)
        {
            var child = _journeyState.GetChild(childId);
            if (child == null)
            {
                return RedirectToAction(nameof(ErrorController.NotFound), "Error");
            }

            return View(new ChildIsBornViewModel(child) { ReturnTo = returnTo });
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
                return RedirectToAction(nameof(BornChildDetailsController.ChildBirthDate), "BornChildDetails",
                    new { childId = model.ChildId });
            }

            return RedirectToAction(nameof(ExpectedChildDetailsController.ChildDueDate), "ExpectedChildDetails",
                new { childId = model.ChildId });
        }
    }
}

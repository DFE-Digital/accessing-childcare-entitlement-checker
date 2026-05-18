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
        public ViewResult ChildName(string? childId = null, string? returnTo = null)
        {
            return View(new ChildNameViewModel(childId, _journeyState) { ReturnTo = returnTo });
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
            if (model.ReturnTo == "check-your-childrens-details")
            {
                return RedirectToAction(nameof(CheckChildDetailsController.CheckChildDetails), "CheckChildDetails",
                    new { fromChildId = model.ChildId });
            }

            return RedirectToAction(nameof(IntroductionController.IsChildBorn), "Introduction",
                new { childId = model.ChildId });
        }

        [HttpGet]
        public IActionResult IsChildBorn(string childId, string? returnTo = null)
        {
            return View(new ChildIsBornViewModel(childId, _journeyState) { ReturnTo = returnTo });
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

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
        public IActionResult ChildDueDate(string childId, string? returnTo = null)
        {
            var child = _journeyState.GetChild(childId);
            if (child == null)
            {
                return NotFound();
            }

            return View(new ChildDueDateViewModel(child) { ReturnTo = returnTo });
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
            if (model.ReturnTo == "check-your-childrens-details")
            {
                return RedirectToAction(nameof(CheckChildDetailsController.CheckChildDetails), "CheckChildDetails",
                    new { fromChildId = model.ChildId });
            }

            return RedirectToAction(nameof(ExpectedChildRelationship), "ExpectedChildDetails",
                new { childId = model.ChildId });
        }

        [HttpGet]
        public IActionResult ExpectedChildRelationship(string childId, string? returnTo = null)
        {
            var child = _journeyState.GetChild(childId);
            if (child == null)
            {
                return NotFound();
            }

            return View(new ExpectedChildRelationshipViewModel(child) { ReturnTo = returnTo });
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
            if (model.ReturnTo == "check-your-childrens-details")
            {
                return RedirectToAction(nameof(CheckChildDetailsController.CheckChildDetails), "CheckChildDetails",
                    new { fromChildId = model.ChildId });
            }

            return RedirectToAction(nameof(CheckChildDetailsController.CheckChildDetails), "CheckChildDetails",
                new { fromChildId = model.ChildId });
        }
    }
}

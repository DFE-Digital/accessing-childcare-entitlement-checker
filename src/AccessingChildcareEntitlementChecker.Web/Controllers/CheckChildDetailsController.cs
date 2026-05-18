using AccessingChildcareEntitlementChecker.Web.Models.CheckChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers
{
    public class CheckChildDetailsController : Controller
    {
        private readonly JourneyState _journeyState;
        private readonly IJourneySession _journeySession;

        public CheckChildDetailsController(
            JourneyState journeyState,
            IJourneySession journeySession)
        {
            _journeyState = journeyState;
            _journeySession = journeySession;
        }

        [HttpGet]
        public ViewResult CheckChildDetails(string? fromChildId = null)
        {
            return View(new CheckChildDetailsViewModel(_journeyState, fromChildId));
        }

        [HttpGet]
        public IActionResult Delete(string? childId)
        {
            if (childId is null || !_journeyState.Children.TryGetValue(childId, out var child))
            {
                return RedirectToAction(nameof(CheckChildDetailsController.CheckChildDetails));
            }

            return View(new DeleteChildViewModel
            {
                ChildId = childId,
                Name = child.Name
            });
        }

        [HttpPost]
        public IActionResult Delete(DeleteChildViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.DeleteConfirmed != true)
            {
                return RedirectToAction(nameof(CheckChildDetailsController.CheckChildDetails));
            }

            if (_journeyState.Children.Remove(model.ChildId, out var child))
            {
                TempData["DeletedChildName"] = child.Name;
                _journeySession.Set(_journeyState);
            }

            return RedirectToAction(nameof(CheckChildDetailsController.CheckChildDetails));
        }
    }
}

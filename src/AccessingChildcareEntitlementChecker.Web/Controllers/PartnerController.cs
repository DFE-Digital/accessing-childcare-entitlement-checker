using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class PartnerController : Controller
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;

    public PartnerController(
        JourneyState journeyState,
        IJourneySession journeySession)
    {
        _journeyState = journeyState;
        _journeySession = journeySession;
    }

        [HttpGet]
        public IActionResult PartnerAge(string? return_to = null)
        {
            return View(new PartnerAgeViewModel(_journeyState) { ReturnTo = return_to });
        }

        [HttpPost]
        public IActionResult PartnerAge(PartnerAgeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _journeyState.Apply(model);
            _journeySession.Set(_journeyState);
            if (model.ReturnTo == "check-your-childrens-details")
            {
                return RedirectToAction(nameof(CheckChildDetailsController.CheckChildDetails), "CheckChildDetails");
            }

            return RedirectToAction(nameof(UserController.NextStepPlaceholder), "User");
        }
}

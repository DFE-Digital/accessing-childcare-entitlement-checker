using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using AccessingChildcareEntitlementChecker.Web.Extensions;

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
        public IActionResult ChildBirthDate(string childId, string? returnTo = null)
        {
            var child = _journeyState.GetChild(childId);
            if (child == null)
            {
                return NotFound();
            }

            return View(new ChildBirthDateViewModel(child) { ReturnTo = returnTo });
        }

        [HttpPost]
        public IActionResult ChildBirthDate(ChildBirthDateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _journeyState.Apply(model);
            _journeySession.Set(_journeyState);
            if (model.ReturnTo == "check-your-childrens-details")
            {
                return this.RedirectTo<CheckChildDetailsController>(
                    nameof(CheckChildDetailsController.CheckChildDetails),
                    new { fromChildId = model.ChildId });
            }

            return this.RedirectTo<BornChildDetailsController>(
                nameof(ChildRelationship),
                new { childId = model.ChildId });
        }

        [HttpGet]
        public IActionResult ChildRelationship(string childId, string? returnTo = null)
        {
            var child = _journeyState.GetChild(childId);
            if (child == null)
            {
                return NotFound();
            }

            return View(new ChildRelationshipViewModel(child) { ReturnTo = returnTo });
        }

        [HttpPost]
        public IActionResult ChildRelationship(ChildRelationshipViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _journeyState.Apply(model);
            _journeySession.Set(_journeyState);
            if (model.ReturnTo == "check-your-childrens-details")
            {
                return this.RedirectTo<CheckChildDetailsController>(
                    nameof(CheckChildDetailsController.CheckChildDetails),
                    new { fromChildId = model.ChildId });
            }

            return this.RedirectTo<BornChildDetailsController>(
                nameof(ChildSupport),
                new { childId = model.ChildId });
        }

        [HttpGet]
        public IActionResult ChildSupport(string childId, string? returnTo = null)
        {
            var child = _journeyState.GetChild(childId);
            if (child == null)
            {
                return NotFound();
            }

            return View(new ChildSupportViewModel(child) { ReturnTo = returnTo });
        }

        [HttpPost]
        public IActionResult ChildSupport(ChildSupportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _journeyState.Apply(model);
            _journeySession.Set(_journeyState);
            return this.RedirectTo<CheckChildDetailsController>(
                nameof(CheckChildDetailsController.CheckChildDetails),
                new { fromChildId = model.ChildId });
        }
    }
}

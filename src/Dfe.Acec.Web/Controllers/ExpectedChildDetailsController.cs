using Dfe.Acec.Web.Filters;
using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.ExpectedChildDetails;
using Dfe.Acec.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Acec.Web.Controllers;

[ServiceFilter(typeof(RequireJourneySessionFilter))]
public class ExpectedChildDetailsController(
    JourneyState journeyState,
    IJourneySession journeySession) : Controller
{
    public const string Name = "ExpectedChildDetails";

    [HttpGet]
    public IActionResult ChildDueDate(string childId, string? returnTo = null)
    {
        if (!journeyState.Children.TryGetValue(childId, out var child))
        {
            return NotFound();
        }

        var backLink = GetChildDueDateBackLink(childId, returnTo);
        return View(new ChildDueDateViewModel(child, backLink, returnTo));
    }

    [HttpPost]
    public IActionResult ChildDueDate(ChildDueDateViewModel model)
    {
        if (!journeyState.Children.TryGetValue(model.ChildId, out var _))
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.BackLink = GetChildDueDateBackLink(model.ChildId, model.ReturnTo);
            return View(model);
        }

        journeyState.Apply(model);
        journeySession.SetState(journeyState);

        return this.RedirectToAction(
            nameof(SummaryController.CheckChildDetails),
            SummaryController.Name,
            new { childId = model.ChildId });
    }

    private string GetChildDueDateBackLink(string childId, string? returnTo)
    {
        if (ReturnTo.TryGetReturnToUrl(Url, returnTo, childId, out var url))
        {
            return url;
        }

        return this.Url.ActionOrThrow(nameof(IntroductionController.IsChildBorn), IntroductionController.Name, new { childId });
    }
}

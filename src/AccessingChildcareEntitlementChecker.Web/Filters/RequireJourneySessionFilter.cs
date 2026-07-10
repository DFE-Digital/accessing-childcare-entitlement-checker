using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AccessingChildcareEntitlementChecker.Web.Filters;

public class RequireJourneySessionFilter : IAsyncResourceFilter
{
    private readonly IJourneySession _journeySession;

    public RequireJourneySessionFilter(IJourneySession journeySession)
    {
        _journeySession = journeySession;
    }

    public Task OnResourceExecutionAsync(
        ResourceExecutingContext context,
        ResourceExecutionDelegate next)
    {
        if (!_journeySession.HasSession)
        {
            context.Result = new RedirectToActionResult(
                nameof(HomeController.SessionExpired),
                HomeController.Name,
                null);

            return Task.CompletedTask;
        }

        return next();
    }
}

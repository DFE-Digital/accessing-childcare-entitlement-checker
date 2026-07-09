using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AccessingChildcareEntitlementChecker.Web.Filters;

public class RequireJourneySessionFilter : IAsyncActionFilter
{
    private readonly IJourneySession _journeySession;

    public RequireJourneySessionFilter(IJourneySession journeySession)
    {
        _journeySession = journeySession;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        if (!_journeySession.HasSession)
        {
            context.Result = new RedirectToActionResult(
                nameof(HomeController.SessionExpired),
                HomeController.Name,
                null);

            return;
        }

        await next();
    }
}

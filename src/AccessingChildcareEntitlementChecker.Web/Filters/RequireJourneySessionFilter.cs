using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AccessingChildcareEntitlementChecker.Web.Filters;

public class RequireJourneySessionFilter(
    ILogger<RequireJourneySessionFilter> logger,
    IJourneySession journeySession) : IAsyncResourceFilter
{
    private readonly ILogger<RequireJourneySessionFilter> _logger = logger;
    private readonly IJourneySession _journeySession = journeySession;

    public Task OnResourceExecutionAsync(
        ResourceExecutingContext context,
        ResourceExecutionDelegate next)
    {
        if (_journeySession.HasSession)
        {
            return next();
        }

        _logger.LogInformation(
            "Redirecting session-less request for {Path} to SessionExpired.",
            context.HttpContext.Request.Path);
        context.Result = new RedirectToActionResult(
            nameof(HomeController.SessionExpired),
            HomeController.Name,
            null);

        return Task.CompletedTask;
    }
}

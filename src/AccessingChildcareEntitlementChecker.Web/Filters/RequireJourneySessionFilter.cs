using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AccessingChildcareEntitlementChecker.Web.Filters;

public partial class RequireJourneySessionFilter(
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

        LogRedirectingSessionLessRequest(_logger, context.HttpContext.Request.Path);
        context.Result = new RedirectToActionResult(
            nameof(HomeController.SessionExpired),
            HomeController.Name,
            null);

        return Task.CompletedTask;
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "Redirecting session-less request for {Path} to SessionExpired.")]
    private static partial void LogRedirectingSessionLessRequest(ILogger logger, PathString path);
}

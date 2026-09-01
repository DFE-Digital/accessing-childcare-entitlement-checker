using Dfe.Acec.Web.Controllers;
using Dfe.Acec.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dfe.Acec.Web.Filters;

public partial class RequireJourneySessionFilter(
    ILogger<RequireJourneySessionFilter> logger,
    IJourneySession journeySession) : IAsyncResourceFilter
{
    public Task OnResourceExecutionAsync(
        ResourceExecutingContext context,
        ResourceExecutionDelegate next)
    {
        if (journeySession.HasSession)
        {
            return next();
        }

        LogRedirectingSessionLessRequest(context.HttpContext.Request.Path);
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
    private partial void LogRedirectingSessionLessRequest(PathString path);
}

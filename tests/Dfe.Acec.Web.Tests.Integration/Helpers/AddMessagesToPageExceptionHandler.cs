using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Dfe.Acec.Web.Tests.Integration.Helpers;

public class AddMessagesToPageExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(new
        {
            StatusCode = httpContext.Response.StatusCode,
            Message = exception.Message,
            Type = exception.GetType().Name,
            StackTrace = exception.StackTrace
        }, cancellationToken);

        return true;
    }
}

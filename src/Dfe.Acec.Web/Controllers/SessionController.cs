using Dfe.Acec.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Acec.Web.Controllers;

public class SessionController(IJourneySession journeySession) : Controller
{
    public const string Name = "Session";

    [HttpPost]
    [SkipStatusCodePages]
    public IActionResult KeepAlive()
    {
        return journeySession.HasSession ? NoContent() : StatusCode(StatusCodes.Status410Gone);
    }
}

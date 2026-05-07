using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

[Route("Error")]
public class ErrorController : Controller
{
    [Route("")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public ViewResult InternalServerError()
    {
        Response.StatusCode = 500;
        return View();
    }

    [Route("{statusCode:int}")]
    [SuppressMessage("SonarQube", "S6967", Justification = "Route constraint :int guarantees a valid integer; ModelState check is redundant.")]
    public ViewResult StatusCodePage(int statusCode)
    {

        Response.StatusCode = statusCode;
        return statusCode switch
        {
            404 => View("NotFound"),
            _ => View("InternalServerError"),
        };
    }
}
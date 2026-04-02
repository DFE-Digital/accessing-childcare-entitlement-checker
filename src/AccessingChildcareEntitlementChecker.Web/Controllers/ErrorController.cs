using System.Diagnostics;
using AccessingChildcareEntitlementChecker.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

[Route("Error")]
public class ErrorController : Controller
{
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var traceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

        _logger.LogError("Unhandled error occurred. TraceIdentifier: {TraceId}", traceId);

        Response.StatusCode = 500;

        return View();
    }
}
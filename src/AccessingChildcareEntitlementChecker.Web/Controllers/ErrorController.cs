using System.Diagnostics;
using AccessingChildcareEntitlementChecker.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

[Route("Error")]
public class ErrorController : Controller
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Index()
    {
        return View("Error", new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
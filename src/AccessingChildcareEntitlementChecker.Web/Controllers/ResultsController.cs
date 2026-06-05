using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace AccessingChildcareEntitlementChecker.Web.Controllers
{
    public class ResultsController : Controller
    {
        [HttpGet]
        [ExcludeFromCodeCoverage(Justification = "This page is a stub for a future page")]
        public IActionResult Results()
        {
            return Content("<h1>Childcare support you could get</h1>", "text/html");
        }
    }
}

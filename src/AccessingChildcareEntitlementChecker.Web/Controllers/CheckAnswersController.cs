using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers;

public class CheckAnswersController : Controller
{
    [HttpGet]
    public IActionResult CheckAnswers()
    {
        return Content("<h1>Check your answers</h1>", "text/html");
    }
}

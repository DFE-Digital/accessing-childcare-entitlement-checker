using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers
{
    public class ChildDetailsController : Controller
    {
        public ChildDetailsController()
        {
        }

        [HttpGet]
        public ViewResult ChildBirthDate()
        {
            return View();
        }

        [HttpGet]
        public ViewResult ChildDueDate()
        {
            return View();
        }
    }
}

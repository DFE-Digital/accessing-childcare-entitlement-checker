using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Controllers
{
    public class BornChildDetailsController : Controller
    {
        public BornChildDetailsController()
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

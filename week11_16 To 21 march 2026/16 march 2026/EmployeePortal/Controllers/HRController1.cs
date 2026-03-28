using Microsoft.AspNetCore.Mvc;
using EmployeePortal.Filters;
using EmployeePortal.Controllers;

namespace EmployeePortal.Controllers
{
    [ServiceFilter(typeof(LogActionFilter))]
    public class HRController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult EmployeeList()
        {
            var list = EmployeeController.GetEmployees();
            return View(list);
        }

        public IActionResult Reports()
        {
            throw new Exception("Test Exception in Reports");
        }
    }
}
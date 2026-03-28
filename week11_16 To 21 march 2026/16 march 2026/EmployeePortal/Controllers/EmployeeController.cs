using Microsoft.AspNetCore.Mvc;
using EmployeePortal.Models;

namespace EmployeePortal.Controllers
{
    public class EmployeeController : Controller
    {
        private static List<Employee> employees = new List<Employee>();

        // GET: Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public IActionResult Register(Employee emp)
        {
            if (ModelState.IsValid)
            {
                emp.Id = employees.Count + 1;
                employees.Add(emp);

                TempData["Success"] = "Employee Registered Successfully!";

                // 🔥 IMPORTANT REDIRECT
                return RedirectToAction("Details", new { id = emp.Id });
            }

            return View(emp);
        }

        // DETAILS PAGE
        public IActionResult Details(int id)
        {
            var emp = employees.FirstOrDefault(e => e.Id == id);

            if (emp == null)
                return NotFound();

            return View(emp);
        }

        // FOR HR MODULE
        public static List<Employee> GetEmployees()
        {
            return employees;
        }
    }
}
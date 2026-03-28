using Microsoft.AspNetCore.Mvc;
using Student_Registration.Models;

namespace Student_Registration.Controllers
{
    public class StudentController : Controller
    {
        // Temporary ID generator
        static int studentId = 1;

        // GET: Register Page
        public IActionResult Register()
        {
            return View();
        }

        // POST: Form submission
        [HttpPost]
        public IActionResult Register(Student student)
        {
            // Model Binding automatically fills Student object

            if (ModelState.IsValid)
            {
                student.Id = studentId++;

                // Success message
                TempData["SuccessMessage"] = "Student registered successfully!";

                // Redirect to Details page
                return RedirectToAction("Details", new { id = student.Id });
            }

            // If validation fails
            return View(student);
        }

        // Details Page
        public IActionResult Details(int id)
        {
            ViewBag.StudentId = id;
            return View();
        }
    }
}

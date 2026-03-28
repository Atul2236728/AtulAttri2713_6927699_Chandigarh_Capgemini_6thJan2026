using Microsoft.AspNetCore.Mvc;

namespace StudentManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username.ToLower() == "atul" && password == "123")
            {
                HttpContext.Session.SetString("Username", username);
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid Credentials";
            return View();
        }

        public IActionResult Dashboard()
        {
            var user = HttpContext.Session.GetString("Username");

            if (user == null)
                return RedirectToAction("Login");

            ViewBag.Username = user;
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
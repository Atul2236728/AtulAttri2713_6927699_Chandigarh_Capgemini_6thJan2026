using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Session_Login_System.Controllers
{
    public class AccountController : Controller
    {
        // GET: Login Page
        public IActionResult Login()
        {
            // If already logged in → go to dashboard
            if (HttpContext.Session.GetString("Username") != null)
            {
                return RedirectToAction("Dashboard");
            }

            return View();
        }

        // POST: Login Logic
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == "admin" && password == "123")
            {
                // Store session
                HttpContext.Session.SetString("Username", username);

                // Success message
                TempData["Success"] = "Login Successful!";

                return RedirectToAction("Dashboard");
            }

            // Error message
            ViewBag.Error = "Invalid Username or Password";

            return View();
        }

        // Dashboard
        public IActionResult Dashboard()
        {
            var username = HttpContext.Session.GetString("Username");

            // Security check
            if (username == null)
            {
                return RedirectToAction("Login");
            }

            ViewBag.Username = username;

            return View();
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            TempData["Logout"] = "You have been logged out successfully.";

            return RedirectToAction("Login");
        }
    }
}
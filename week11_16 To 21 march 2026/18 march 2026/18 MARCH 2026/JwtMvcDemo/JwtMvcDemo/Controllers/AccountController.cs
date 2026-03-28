using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtMvcDemo.Models;
using Microsoft.AspNetCore.Http;

namespace JwtMvcDemo.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _config;

        public AccountController(IConfiguration config)
        {
            _config = config;
        }

        // 🔹 Login Page
        public IActionResult Login()
        {
            return View();
        }

        // 🔹 Login POST
        [HttpPost]
        public IActionResult Login(User user)
        {
            // Dummy authentication (replace with DB later)
            if (user.Username == "admin" && user.Password == "1234")
            {
                var token = GenerateToken(user);

                // Store token in session
                HttpContext.Session.SetString("JWToken", token);

                return RedirectToAction("Dashboard");
            }

            ViewBag.Message = "Invalid Login!";
            return View();
        }

        // 🔹 Protected Dashboard
        public IActionResult Dashboard()
        {
            var token = HttpContext.Session.GetString("JWToken");

            // If token not found → redirect to login
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        // 🔹 Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWToken");
            return RedirectToAction("Login");
        }

        // 🔐 JWT Token Generator
        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
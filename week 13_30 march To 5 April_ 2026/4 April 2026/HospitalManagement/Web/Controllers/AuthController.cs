using System.Security.Claims;
using HospitalWeb.Models;
using HospitalWeb.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HospitalWeb.Controllers;

public class AuthController : Controller
{
    private readonly ApiService _api;

    public AuthController(ApiService api) => _api = api;

    [HttpGet]
    public IActionResult Login(string? returnUrl)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToRoleHome();
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var (success, json, error) = await _api.PostAsync("api/auth/login", new
        {
            email = model.Email,
            password = model.Password
        });

        if (!success || json == null)
        {
            ModelState.AddModelError("", error ?? "Invalid credentials.");
            return View(model);
        }

        var auth = JsonConvert.DeserializeObject<AuthResponse>(json)!;

        // Store token in session
        HttpContext.Session.SetString("JwtToken", auth.Token);
        HttpContext.Session.SetString("UserRole", auth.Role);
        HttpContext.Session.SetInt32("UserId", auth.UserId);
        HttpContext.Session.SetString("UserName", auth.FullName);
        if (auth.DoctorId.HasValue)
            HttpContext.Session.SetInt32("DoctorId", auth.DoctorId.Value);

        // Cookie auth for MVC
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, auth.UserId.ToString()),
            new(ClaimTypes.Name, auth.FullName),
            new(ClaimTypes.Email, auth.Email),
            new(ClaimTypes.Role, auth.Role),
            new("UserId", auth.UserId.ToString()),
            new("DoctorId", auth.DoctorId?.ToString() ?? "0"),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24) });

        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            return Redirect(model.ReturnUrl);

        return RedirectToRoleHome(auth.Role);
    }

    [HttpGet]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var (success, json, error) = await _api.PostAsync("api/auth/register", new
        {
            fullName = model.FullName,
            email = model.Email,
            password = model.Password,
            phone = model.Phone
        });

        if (!success || json == null)
        {
            ModelState.AddModelError("", error ?? "Registration failed.");
            return View(model);
        }

        var auth = JsonConvert.DeserializeObject<AuthResponse>(json)!;

        HttpContext.Session.SetString("JwtToken", auth.Token);
        HttpContext.Session.SetString("UserRole", auth.Role);
        HttpContext.Session.SetInt32("UserId", auth.UserId);
        HttpContext.Session.SetString("UserName", auth.FullName);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, auth.UserId.ToString()),
            new(ClaimTypes.Name, auth.FullName),
            new(ClaimTypes.Email, auth.Email),
            new(ClaimTypes.Role, auth.Role),
            new("UserId", auth.UserId.ToString()),
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));

        return RedirectToAction("Index", "Patient");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    public IActionResult AccessDenied() => View();

    private IActionResult RedirectToRoleHome(string? role = null)
    {
        role ??= User.FindFirst(ClaimTypes.Role)?.Value;
        return role switch
        {
            "Admin" => RedirectToAction("Index", "Admin"),
            "Doctor" => RedirectToAction("Index", "Doctor"),
            _ => RedirectToAction("Index", "Patient")
        };
    }
}

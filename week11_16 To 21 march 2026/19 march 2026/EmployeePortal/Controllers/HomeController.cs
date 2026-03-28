using System.Diagnostics;
using EmployeePortal.Data;
using EmployeePortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace EmployeePortal.Controllers
{
	public class HomeController : Controller
	{
		
		private readonly ApplicationDbContext _context;

		public HomeController(ApplicationDbContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}

		[Authorize]
		public IActionResult Dashboard()
		{
			var totalEmployees = _context.Employees.Count();

			var totalDepartments = _context.Employees
				.Select(e => e.Department)
				.Distinct()
				.Count();

			ViewBag.TotalEmployees = totalEmployees;
			ViewBag.TotalDepartments = totalDepartments;

			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}

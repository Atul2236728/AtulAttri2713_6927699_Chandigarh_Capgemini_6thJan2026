using EmployeePortal.Data;
using EmployeePortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class EmployeeController : Controller
{
	private readonly ApplicationDbContext _context;

	public EmployeeController(ApplicationDbContext context)
	{
		_context = context;
	}

	[Authorize]
	public async Task<IActionResult> Index(string search)
	{
		var employees = _context.Employees.AsQueryable();

		if (!string.IsNullOrEmpty(search))
		{
			employees = employees.Where(e =>
				e.Name.Contains(search) || e.Department.Contains(search));
		}

		return View(await employees.ToListAsync());
	}

	[Authorize(Roles = "Admin")]
	public IActionResult Create()
	{
		return View();
	}

	[HttpPost]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Create(Employee emp)
	{
		if (ModelState.IsValid)
		{
			_context.Add(emp);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		return View(emp);
	}

	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Edit(int id)
	{
		var emp = await _context.Employees.FindAsync(id);
		return View(emp);
	}

	[HttpPost]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Edit(Employee emp)
	{
		if (ModelState.IsValid)
		{
			_context.Update(emp);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		return View(emp);
	}

	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Delete(int id)
	{
		var emp = await _context.Employees.FindAsync(id);
		return View(emp);
	}

	[HttpPost, ActionName("Delete")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var emp = await _context.Employees.FindAsync(id);
		_context.Remove(emp);
		await _context.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}
}
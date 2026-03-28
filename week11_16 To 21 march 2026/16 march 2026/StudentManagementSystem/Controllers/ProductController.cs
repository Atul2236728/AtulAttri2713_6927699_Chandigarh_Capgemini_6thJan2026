using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Filters;
using StudentManagementSystem.Models;
using System.Collections.Generic;

namespace StudentManagementSystem.Controllers
{
    [ServiceFilter(typeof(LogActionFilter))]
    public class ProductController : Controller
    {
        static List<Product> products = new List<Product>();

        public IActionResult Index()
        {
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            products.Add(product);
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var product = products.Find(p => p.Id == id);
            return View(product);
        }
    }
}
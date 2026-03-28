using Microsoft.AspNetCore.Mvc;
using Product_Management_Module.Filters;
using System;
using System.Collections.Generic;

namespace Product_Management_Module.Controllers
{
    [ServiceFilter(typeof(LogActionFilter))]
    public class ProductController : Controller
    {
        public IActionResult Index(bool testError = false)
        {
            if (testError)
            {
                throw new Exception("Test Exception");
            }

            var products = new List<string>
            {
                "Laptop",
                "Mobile",
                "Keyboard",
                "Mouse"
            };

            return View(products);
        }

        public IActionResult Details(string name)
        {
            List<string> brands = new List<string>();

            if (name == "Laptop")
            {
                brands = new List<string> { "Dell", "HP", "Lenovo" };
            }
            else if (name == "Mobile")
            {
                brands = new List<string> { "Samsung", "Apple", "OnePlus" };
            }
            else if (name == "Keyboard")
            {
                brands = new List<string> { "Logitech", "Corsair", "Redragon" };
            }
            else if (name == "Mouse")
            {
                brands = new List<string> { "Logitech", "Razer", "HP" };
            }

            ViewBag.ProductName = name;
            ViewBag.Brands = brands;

            return View();
        }
    }
}
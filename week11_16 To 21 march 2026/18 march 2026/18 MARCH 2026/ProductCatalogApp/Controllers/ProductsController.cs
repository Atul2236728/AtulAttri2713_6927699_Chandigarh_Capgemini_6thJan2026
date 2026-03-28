using Microsoft.AspNetCore.Mvc;
using ProductCatalogApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductCatalogApp.Controllers
{
    public class ProductsController : Controller
    {
        private static List<Product> products = new List<Product>()
        {
            new Product { Id = 1, Name = "Laptop", Description = "High performance laptop", Price = 80000, ImageUrl = "https://via.placeholder.com/300" },
            new Product { Id = 2, Name = "Phone", Description = "Latest smartphone", Price = 50000, ImageUrl = "https://via.placeholder.com/300" },
            new Product { Id = 3, Name = "Headphones", Description = "Noise cancelling", Price = 5000, ImageUrl = "https://via.placeholder.com/300" }
        };

        public IActionResult Index(string search)
        {
            var result = string.IsNullOrEmpty(search)
                ? products
                : products.Where(p => p.Name.ToLower().Contains(search.ToLower())).ToList();

            return View(result);
        }

        public IActionResult Details(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            return View(product);
        }
    }
}
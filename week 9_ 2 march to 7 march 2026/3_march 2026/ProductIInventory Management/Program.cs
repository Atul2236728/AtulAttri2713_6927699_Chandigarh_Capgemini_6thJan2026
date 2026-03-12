using System;
using System.Collections.Generic;
using System.Linq;

public interface IProduct
{
    string Name { get; set; }
    string Category { get; set; }
    int Stock { get; set; }
    int Price { get; set; }
}

public interface IInventory
{
    void AddProduct(IProduct product);
    void RemoveProduct(IProduct product);
    int CalculateTotalValue();
    List<IProduct> GetProductsByCategory(string category);
    List<IProduct> SearchProductsByName(string name);
    List<(string, int)> GetProductsByCategoryWithCount();
    List<(string, List<IProduct>)> GetAllProductsByCategory();
}

public class Product : IProduct
{
    public string Name { get; set; }
    public string Category { get; set; }
    public int Stock { get; set; }
    public int Price { get; set; }
}

public class Inventory : IInventory
{
    private List<IProduct> products = new List<IProduct>();

    public void AddProduct(IProduct product)
    {
        products.Add(product);
    }

    public void RemoveProduct(IProduct product)
    {
        products.Remove(product);
    }

    public int CalculateTotalValue()
    {
        return products.Sum(p => p.Price * p.Stock);
    }

    public List<IProduct> GetProductsByCategory(string category)
    {
        return products.Where(p => p.Category.ToLower() == category.ToLower()).ToList();
    }

    public List<IProduct> SearchProductsByName(string name)
    {
        return products.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList();
    }

    public List<(string, int)> GetProductsByCategoryWithCount()
    {
        return products
            .GroupBy(p => p.Category)
            .Select(g => (g.Key, g.Count()))
            .ToList();
    }

    public List<(string, List<IProduct>)> GetAllProductsByCategory()
    {
        return products
            .GroupBy(p => p.Category)
            .Select(g => (g.Key, g.ToList()))
            .ToList();
    }
}

class Solution
{
    static void Main()
    {
        IInventory inventory = new Inventory();

        Console.Write("Enter number of products: ");
        int pCount = Convert.ToInt32(Console.ReadLine());

        for (int i = 1; i <= pCount; i++)
        {
            Console.WriteLine("\nEnter product details:");

            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Category: ");
            string category = Console.ReadLine();

            Console.Write("Stock: ");
            int stock = Convert.ToInt32(Console.ReadLine());

            Console.Write("Price: ");
            int price = Convert.ToInt32(Console.ReadLine());

            Product p = new Product()
            {
                Name = name,
                Category = category,
                Stock = stock,
                Price = price
            };

            inventory.AddProduct(p);
        }

        Console.Write("\nEnter category to display products: ");
        string categoryName = Console.ReadLine();

        Console.WriteLine("\nProducts in Category:");

        var byCategory = inventory.GetProductsByCategory(categoryName);

        foreach (var product in byCategory.OrderBy(x => x.Name))
        {
            Console.WriteLine($"Product Name:{product.Name} Category:{product.Category}");
        }

        Console.Write("\nEnter product name to search: ");
        string searchName = Console.ReadLine();

        Console.WriteLine("\nSearch Result:");

        var searchResult = inventory.SearchProductsByName(searchName);

        foreach (var product in searchResult.OrderBy(x => x.Name))
        {
            Console.WriteLine($"Product Name:{product.Name} Category:{product.Category}");
        }

        Console.WriteLine($"\nTotal Value:${inventory.CalculateTotalValue()}");

        Console.WriteLine("\nProducts Count by Category:");

        var categoryCount = inventory.GetProductsByCategoryWithCount();

        foreach (var item in categoryCount.OrderBy(x => x.Item1))
        {
            Console.WriteLine($"{item.Item1}:{item.Item2}");
        }

        Console.WriteLine("\nAll Products Grouped by Category:");

        var grouped = inventory.GetAllProductsByCategory();

        foreach (var group in grouped.OrderBy(x => x.Item1))
        {
            Console.WriteLine($"{group.Item1}:");

            foreach (var product in group.Item2)
            {
                Console.WriteLine($"Product Name:{product.Name} Price:{product.Price}");
            }
        }

        Console.Write("\nEnter product name to delete: ");
        string deleteName = Console.ReadLine();

        var toDelete = inventory.SearchProductsByName(deleteName);

        foreach (var product in toDelete)
        {
            inventory.RemoveProduct(product);
        }

        Console.WriteLine($"\nNew Total Value:${inventory.CalculateTotalValue()}");
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

public interface IProduct
{
    int Id { get; set; }
    string Name { get; set; }
    decimal Price { get; set; }
}

public interface ICategory
{
    int Id { get; set; }
    string Name { get; set; }
    List<IProduct> Products { get; set; }
}

public class Product : IProduct
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class Category : ICategory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<IProduct> Products { get; set; } = new List<IProduct>();
}

class Solution
{
    static void Main()
    {
        List<Product> products = new List<Product>();
        List<Category> categories = new List<Category>();

        Console.Write("Enter number of products: ");
        int p = int.Parse(Console.ReadLine());

        for (int i = 0; i < p; i++)
        {
            Console.Write("Product Id: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Product Name: ");
            string name = Console.ReadLine();

            Console.Write("Price: ");
            decimal price = decimal.Parse(Console.ReadLine());

            products.Add(new Product { Id = id, Name = name, Price = price });
        }

        Console.Write("\nEnter number of categories: ");
        int c = int.Parse(Console.ReadLine());

        for (int i = 0; i < c; i++)
        {
            Console.Write("Category Id: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Category Name: ");
            string name = Console.ReadLine();

            categories.Add(new Category { Id = id, Name = name });
        }

        Console.Write("\nEnter number of product-category relations: ");
        int r = int.Parse(Console.ReadLine());

        for (int i = 0; i < r; i++)
        {
            Console.Write("Category Id: ");
            int cid = int.Parse(Console.ReadLine());

            Console.Write("Product Id: ");
            int pid = int.Parse(Console.ReadLine());

            var cat = categories.First(x => x.Id == cid);
            var prod = products.First(x => x.Id == pid);

            cat.Products.Add(prod);
        }

        Console.WriteLine("\nTop category by product count:");
        var top = categories.OrderByDescending(x => x.Products.Count).First();
        Console.WriteLine(top.Name);

        Console.WriteLine("\nCategory total values:");
        foreach (var cat in categories)
        {
            decimal total = cat.Products.Sum(p1 => p1.Price);
            Console.WriteLine($"{cat.Name} {total}");
        }
    }
}
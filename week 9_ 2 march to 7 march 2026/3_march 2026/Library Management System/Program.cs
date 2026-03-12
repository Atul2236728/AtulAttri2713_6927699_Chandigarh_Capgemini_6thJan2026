using System;
using System.Collections.Generic;
using System.Linq;

public interface IBook
{
    int Id { get; set; }
    string Title { get; set; }
    string Author { get; set; }
    string Category { get; set; }
    int Price { get; set; }
}

public interface ILibrarySystem
{
    void AddBook(IBook book, int quantity);
    void RemoveBook(IBook book, int quantity);
    int CalculateTotal();
    List<(string, int)> CategoryTotalPrice();
    List<(string, int, int)> BooksInfo();
    List<(string, string, int)> CategoryAndAuthorWithCount();
}

public class Book : IBook
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Category { get; set; }
    public int Price { get; set; }
}

public class LibrarySystem : ILibrarySystem
{
    private Dictionary<IBook, int> books = new Dictionary<IBook, int>();

    public void AddBook(IBook book, int quantity)
    {
        var existing = books.Keys.FirstOrDefault(b => b.Id == book.Id);

        if (existing != null)
        {
            books[existing] += quantity;
        }
        else
        {
            books.Add(book, quantity);
        }
    }

    public void RemoveBook(IBook book, int quantity)
    {
        var existing = books.Keys.FirstOrDefault(b => b.Id == book.Id);

        if (existing != null)
        {
            books[existing] -= quantity;

            if (books[existing] <= 0)
                books.Remove(existing);
        }
    }

    public int CalculateTotal()
    {
        int total = 0;

        foreach (var item in books)
        {
            total += item.Key.Price * item.Value;
        }

        return total;
    }

    public List<(string, int)> CategoryTotalPrice()
    {
        return books
            .GroupBy(x => x.Key.Category)
            .Select(g => (g.Key, g.Sum(x => x.Key.Price * x.Value)))
            .ToList();
    }

    public List<(string, int, int)> BooksInfo()
    {
        return books
            .Select(x => (x.Key.Title, x.Value, x.Key.Price))
            .ToList();
    }

    public List<(string, string, int)> CategoryAndAuthorWithCount()
    {
        return books
            .GroupBy(x => new { x.Key.Category, x.Key.Author })
            .Select(g => (g.Key.Category, g.Key.Author, g.Sum(x => x.Value)))
            .ToList();
    }
}

class Solution
{
    static void Main()
    {
        ILibrarySystem librarySystem = new LibrarySystem();

        Console.Write("Enter number of books: ");
        int bCount = Convert.ToInt32(Console.ReadLine());

        for (int i = 1; i <= bCount; i++)
        {
            Console.WriteLine("\nEnter book details:");

            Console.Write("Id: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.Write("Title: ");
            string title = Console.ReadLine();

            Console.Write("Author: ");
            string author = Console.ReadLine();

            Console.Write("Category: ");
            string category = Console.ReadLine();

            Console.Write("Price: ");
            int price = Convert.ToInt32(Console.ReadLine());

            Console.Write("Quantity: ");
            int quantity = Convert.ToInt32(Console.ReadLine());

            IBook book = new Book()
            {
                Id = id,
                Title = title,
                Author = author,
                Category = category,
                Price = price
            };

            librarySystem.AddBook(book, quantity);
        }

        Console.WriteLine("\nBook Info:");

        var booksInfo = librarySystem.BooksInfo();

        foreach (var item in booksInfo.OrderBy(x => x.Item1))
        {
            Console.WriteLine($"Book Name:{item.Item1}, Quantity:{item.Item2}, Price:{item.Item3}");
        }

        Console.WriteLine("\nCategory Total Price:");

        var categoryTotal = librarySystem.CategoryTotalPrice();

        foreach (var item in categoryTotal.OrderBy(x => x.Item1))
        {
            Console.WriteLine($"Category:{item.Item1}, Total Price:{item.Item2}");
        }

        Console.WriteLine("\nCategory And Author With Count:");

        var categoryAuthor = librarySystem.CategoryAndAuthorWithCount();

        foreach (var item in categoryAuthor.OrderBy(x => x.Item1))
        {
            Console.WriteLine($"Category:{item.Item1}, Author:{item.Item2}, Count:{item.Item3}");
        }

        int total = librarySystem.CalculateTotal();

        Console.WriteLine($"\nTotal Price: {total}");
    }
}
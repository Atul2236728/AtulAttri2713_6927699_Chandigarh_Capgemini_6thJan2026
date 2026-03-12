using System;
using System.Collections.Generic;
using System.Linq;

public interface IRealEstateListing
{
    int Id { get; set; }
    string Title { get; set; }
    string Description { get; set; }
    string City { get; set; }
    int Price { get; set; }
}

public interface IRealEstateManager
{
    void AddListing(IRealEstateListing listing);
    void RemoveListing(int id);
    void UpdateListing(IRealEstateListing listing);
    List<IRealEstateListing> GetListings();
    List<IRealEstateListing> SearchListings(string keyword);
}

public class RealEstateListing : IRealEstateListing
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string City { get; set; }
    public int Price { get; set; }
}

public class RealEstateManager : IRealEstateManager
{
    private List<IRealEstateListing> listings = new List<IRealEstateListing>();

    public void AddListing(IRealEstateListing listing)
    {
        var existing = listings.FirstOrDefault(l => l.Id == listing.Id);

        if (existing != null)
        {
            existing.Title = listing.Title;
            existing.Description = listing.Description;
            existing.City = listing.City;
            existing.Price = listing.Price;
        }
        else
        {
            listings.Add(listing);
        }
    }

    public void RemoveListing(int id)
    {
        var listing = listings.FirstOrDefault(l => l.Id == id);

        if (listing != null)
        {
            listings.Remove(listing);
        }
    }

    public void UpdateListing(IRealEstateListing listing)
    {
        var existing = listings.FirstOrDefault(l => l.Id == listing.Id);

        if (existing != null)
        {
            existing.Title = listing.Title;
            existing.Description = listing.Description;
            existing.City = listing.City;
            existing.Price = listing.Price;
        }
    }

    public List<IRealEstateListing> GetListings()
    {
        return listings;
    }

    public List<IRealEstateListing> SearchListings(string keyword)
    {
        return listings
            .Where(l => l.City.ToLower().Contains(keyword.ToLower()) ||
                        l.Title.ToLower().Contains(keyword.ToLower()))
            .ToList();
    }
}

class Solution
{
    static void Main()
    {
        IRealEstateManager manager = new RealEstateManager();

        Console.Write("Enter number of listings: ");
        int listingCount = Convert.ToInt32(Console.ReadLine());

        for (int i = 1; i <= listingCount; i++)
        {
            Console.WriteLine("\nEnter listing details:");

            Console.Write("Id: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.Write("Title: ");
            string title = Console.ReadLine();

            Console.Write("Description: ");
            string description = Console.ReadLine();

            Console.Write("City: ");
            string city = Console.ReadLine();

            Console.Write("Price: ");
            int price = Convert.ToInt32(Console.ReadLine());

            IRealEstateListing listing = new RealEstateListing()
            {
                Id = id,
                Title = title,
                Description = description,
                City = city,
                Price = price
            };

            manager.AddListing(listing);
        }

        Console.WriteLine("\nAll Listings:");

        var listings = manager.GetListings();

        foreach (var listing in listings.OrderBy(x => x.Id))
        {
            Console.WriteLine($"Id:{listing.Id}, Title:{listing.Title}, City:{listing.City}, Price:{listing.Price}");
        }

        Console.Write("\nEnter keyword to search listings: ");
        string keyword = Console.ReadLine();

        Console.WriteLine("\nSearch Result:");

        var result = manager.SearchListings(keyword);

        foreach (var listing in result.OrderBy(x => x.Id))
        {
            Console.WriteLine($"Id:{listing.Id}, Title:{listing.Title}, City:{listing.City}, Price:{listing.Price}");
        }

        Console.Write("\nEnter Id to remove listing: ");
        int removeId = Convert.ToInt32(Console.ReadLine());

        manager.RemoveListing(removeId);

        Console.WriteLine("\nListings After Removal:");

        foreach (var listing in manager.GetListings())
        {
            Console.WriteLine($"Id:{listing.Id}, Title:{listing.Title}, City:{listing.City}, Price:{listing.Price}");
        }
    }
}
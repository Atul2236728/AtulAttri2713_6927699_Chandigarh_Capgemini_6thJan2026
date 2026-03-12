using System;
using System.Collections.Generic;
using System.Linq;

class Car
{
    public int Id;
    public string Brand;
    public string Model;
    public int Price;
}

class Solution
{
    static void Main()
    {
        List<Car> cars = new List<Car>();

        Console.Write("Enter number of cars: ");
        int n = int.Parse(Console.ReadLine());

        for (int i = 0; i < n; i++)
        {
            Car c = new Car();

            Console.Write("Id: ");
            c.Id = int.Parse(Console.ReadLine());

            Console.Write("Brand: ");
            c.Brand = Console.ReadLine();

            Console.Write("Model: ");
            c.Model = Console.ReadLine();

            Console.Write("Price: ");
            c.Price = int.Parse(Console.ReadLine());

            cars.Add(c);
        }

        Console.WriteLine("\nAll Cars:");

        foreach (var car in cars)
        {
            Console.WriteLine($"Id:{car.Id} Brand:{car.Brand} Model:{car.Model} Price:{car.Price}");
        }

        Console.Write("\nEnter brand to search: ");
        string brand = Console.ReadLine();

        var result = cars.Where(c => c.Brand == brand);

        Console.WriteLine("\nSearch Result:");

        foreach (var car in result)
        {
            Console.WriteLine($"Id:{car.Id} Brand:{car.Brand} Model:{car.Model} Price:{car.Price}");
        }
    }
}
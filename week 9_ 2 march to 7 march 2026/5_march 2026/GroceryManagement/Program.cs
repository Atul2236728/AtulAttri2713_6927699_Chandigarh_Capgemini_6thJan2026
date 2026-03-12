using System;
using System.Collections.Generic;

class GroceryReceipt
{
    Dictionary<string, decimal> prices = new Dictionary<string, decimal>();
    Dictionary<string, int> discounts = new Dictionary<string, int>();

    public void AddPrice(string fruit, decimal price)
    {
        prices[fruit] = price;
    }

    public void AddDiscount(string fruit, int discount)
    {
        discounts[fruit] = discount;
    }

    public void Calculate(List<(string fruit, int quantity)> items)
    {
        Console.WriteLine("\nReceipt:");

        foreach (var item in items)
        {
            decimal price = prices[item.fruit];
            decimal total = price * item.quantity;

            if (discounts.ContainsKey(item.fruit))
            {
                total -= total * discounts[item.fruit] / 100;
            }

            Console.WriteLine($"{item.fruit} {price:0.0} {total:0.0}");
        }
    }
}

class Solution
{
    static void Main()
    {
        GroceryReceipt receipt = new GroceryReceipt();

        Console.Write("Enter number of fruits: ");
        int n = int.Parse(Console.ReadLine());

        for (int i = 0; i < n; i++)
        {
            Console.Write("Fruit name: ");
            string fruit = Console.ReadLine();

            Console.Write("Price: ");
            decimal price = decimal.Parse(Console.ReadLine());

            receipt.AddPrice(fruit, price);
        }

        Console.Write("\nEnter number of discounts: ");
        int m = int.Parse(Console.ReadLine());

        for (int i = 0; i < m; i++)
        {
            Console.Write("Fruit name: ");
            string fruit = Console.ReadLine();

            Console.Write("Discount %: ");
            int discount = int.Parse(Console.ReadLine());

            receipt.AddDiscount(fruit, discount);
        }

        Console.Write("\nEnter number of purchased items: ");
        int b = int.Parse(Console.ReadLine());

        List<(string, int)> items = new List<(string, int)>();

        for (int i = 0; i < b; i++)
        {
            Console.Write("Fruit: ");
            string fruit = Console.ReadLine();

            Console.Write("Quantity: ");
            int quantity = int.Parse(Console.ReadLine());

            items.Add((fruit, quantity));
        }

        receipt.Calculate(items);
    }
}
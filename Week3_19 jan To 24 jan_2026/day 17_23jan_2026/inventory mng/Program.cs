namespace inventory_mng
{
    class Book
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }

    class InventoryApp
    {
        static void Main()
        {
            List<Book> inventory = new List<Book>
        {
            new Book { Title = "C# Basics", Price = 450, Stock = 10 },
            new Book { Title = "LINQ Mastery", Price = 650, Stock = 0 },
            new Book { Title = "ASP.NET Core", Price = 550, Stock = 5 }
        };
            inventory.Add(new Book { Title = "DSA in C#", Price = 400, Stock = 8 });

            decimal targetPrice = 500;
            var affordableBooks = inventory
                .Where(book => book.Price < targetPrice)
                .ToList();

            Console.WriteLine("Books cheaper than ₹500:");
            affordableBooks.ForEach(b => Console.WriteLine(b.Title));

            inventory.ForEach(book => book.Price += book.Price * 0.10m);

            inventory.RemoveAll(book => book.Stock == 0);

            Console.WriteLine("\nFinal Inventory:");
            inventory.ForEach(b =>
                Console.WriteLine($"{b.Title} - ₹{b.Price} - Stock: {b.Stock}")
            );
        }
    }
}

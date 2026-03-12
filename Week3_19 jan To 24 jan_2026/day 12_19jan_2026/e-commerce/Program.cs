namespace e_commerce
{
    class Program
    {
        static void Main()
        {
            Electronics e = new Electronics("Laptop", 50000, 2);
            Books b = new Books("C# Book", 700, 3);

            Cart c = new Cart();
            c.Add(e);
            c.Add(b);
            c.Show();
        }
    }
}

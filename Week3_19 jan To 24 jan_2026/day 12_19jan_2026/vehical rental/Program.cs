namespace vehical_rental
{
    class Program
    {
        static void Main()
        {
            Car c = new Car("Swift", 1000);
            Console.WriteLine("Total Rent: " + c.Calculate(3));
        }
    }
}

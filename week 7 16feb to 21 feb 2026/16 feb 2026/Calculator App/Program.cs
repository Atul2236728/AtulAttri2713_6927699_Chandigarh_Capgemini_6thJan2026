namespace Calculator_App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var cal = new Calculator();
            Console.WriteLine($"Add: {cal.Add(5, 3)}");
            Console.WriteLine($"Subtract: {cal.Subtract(5,3)}");
            Console.WriteLine($"Multiply: {cal.Multiply(5,3)}");
            Console.WriteLine($"Divide: {cal.Divide(10, 2)}");
        }
    }
}

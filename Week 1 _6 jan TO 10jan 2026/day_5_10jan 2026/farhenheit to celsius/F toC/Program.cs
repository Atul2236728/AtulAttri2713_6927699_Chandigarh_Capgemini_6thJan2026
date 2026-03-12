namespace F_toC
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter temperature in Fahrenheit: ");
            double f = Convert.ToDouble(Console.ReadLine());

            double output = FahrenheitToCelsius(f);

            Console.WriteLine("Output: " + output);
        }

        static double FahrenheitToCelsius(double f)
        {
            if (f < 0)
                return -1;

            return (f - 32) * 5 / 9;
        }
    }
}



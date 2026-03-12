namespace Leap_Year
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Enter the year:");
            int year = int.Parse(Console.ReadLine());

            int output;

            if (year < 0)
            {
                output = -1;
            }
            else
            {
                if ((year % 400 == 0) || (year % 4 == 0 && year % 100 != 0))
                    output = 1;  
                else
                    output = 0;   
            }

            Console.WriteLine("Output:");
            Console.WriteLine(output);
        }
    }
}

namespace Gross_Salary
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Enter basic salary:");
            int basicPay = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter number of working days:");
            int days = int.Parse(Console.ReadLine());

            int output;

            if (basicPay < 0)
            {
                output = -1;
            }
            else if (basicPay > 10000)
            {
                output = -2;
            }
            else if (days > 31)
            {
                output = -3;
            }
            else
            {
                double da = basicPay * 0.75;
                double hra = basicPay * 0.50;
                double grossSalary = basicPay + da + hra;
                output = (int)grossSalary;
            }

            Console.WriteLine("Output:");
            Console.WriteLine(output);
        }
    }
}

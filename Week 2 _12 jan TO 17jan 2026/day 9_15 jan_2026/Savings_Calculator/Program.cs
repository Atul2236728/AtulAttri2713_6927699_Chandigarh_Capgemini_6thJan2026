namespace Savings_Calculator
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Enter the Salary :");
            int salary = int.Parse(Console.ReadLine());   // Input1
            Console.WriteLine("Enter the days");
            int days = int.Parse(Console.ReadLine());     // Input2
            int output;

            if (salary > 9000)
            {
                output = -1;
            }
            else if (salary < 0)
            {
                output = -2;
            }
            else if (days < 0)
            {
                output = -4;
            }
            else
            {
                double food = salary * 0.50;
                double travel = salary * 0.20;
                double expenses = food + travel;

                double savings = salary - expenses;

                if (days == 31)
                    savings += 500;

                output = (int)savings;
            }

            Console.WriteLine("Output"+output);
        }
    }
}

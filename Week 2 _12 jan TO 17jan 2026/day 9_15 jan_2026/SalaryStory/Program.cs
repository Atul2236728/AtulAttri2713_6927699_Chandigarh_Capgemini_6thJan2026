namespace SalaryStory
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Enter salary:");
            int salary = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter number of working days:");
            int days = int.Parse(Console.ReadLine());

            int output;

            if (salary > 9000)
            {
                output = -1;
            }
            else if (salary < 0)
            {
                output = -3;
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

            Console.WriteLine("Output:");
            Console.WriteLine(output);
        }
    }
}

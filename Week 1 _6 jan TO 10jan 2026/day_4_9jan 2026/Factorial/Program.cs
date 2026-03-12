namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int output1;
            Console.WriteLine("enter the number ");
            int input1 = convert.ToInt32(Console.ReadLine());

            if (input1 < 0)
            {
                output1 = -1;
            }
            else if (input1 > 999)
            {
                output1 = -2;
            }
            else
            {
                int original = input1;
                int sum = 0;

                while (input1 > 0)
                {
                    int digit = input1 % 10;
                    sum += digit * digit * digit;
                    input1 /= 10;
                }

                if (sum == original)
                    output1 = 1;
                else
                    output1 = 0;
            }

            Console.WriteLine("Output1 = " + output1);
        }
    }
}

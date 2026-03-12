namespace sum
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int num = 2468;
            int output;

            if (num < 0)
                output = -1;
            else if (num > 32767)
                output = -2;
            else
            {
                int sum = 0;
                while (num > 0)
                {
                    int digit = num % 10;
                    if (digit % 2 == 0)
                        sum += digit;
                    num /= 10;
                }
                output = sum;
            }

            Console.WriteLine(output);
        }
    }
}

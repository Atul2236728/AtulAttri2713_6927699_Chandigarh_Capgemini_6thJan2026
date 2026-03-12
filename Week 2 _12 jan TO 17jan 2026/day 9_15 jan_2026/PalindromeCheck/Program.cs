namespace PalindromeCheck
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Enter the number:");
            int input = int.Parse(Console.ReadLine());

            int output;

            if (input < 0)
            {
                output = -1;
            }
            else
            {
                int original = input;
                int reverse = 0;

                while (input > 0)
                {
                    int digit = input % 10;
                    reverse = reverse * 10 + digit;
                    input /= 10;
                }

                if (original == reverse)
                    output = 1;
                else
                    output = -2;
            }

            Console.WriteLine("Output:");
            Console.WriteLine(output);
        }
    }
}

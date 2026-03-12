namespace SumOfFactorsUptoN
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Enter input1:");
            int input1 = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter input2:");
            int input2 = int.Parse(Console.ReadLine());

            int output;

            if (input1 < 0)
            {
                output = -1;
            }
            else if (input2 > 32627)
            {
                output = -2;
            }
            else
            {
                int sum = 0;

                for (int i = input1; i <= input2; i += input1)
                {
                    sum += i;
                }

                output = sum;
            }

            Console.WriteLine("Output:");
            Console.WriteLine(output);
        }
    }
}

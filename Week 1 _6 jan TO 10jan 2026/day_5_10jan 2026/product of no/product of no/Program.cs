namespace product_of_no_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int input1 = 56;
            int output;

            if (input1 < 0)
            {
                output = -1;
            }
            else if (input1 % 3 == 0 || input1 % 5 == 0)
            {
                output = -2;
            }
            else
            {
                int product = 1;
                int num = input1;

                while (num > 0)
                {
                    product *= num % 10;
                    num /= 10;
                }

                if(product % 3 == 0 || product % 5 == 0) ;
                {
                    output = 1;
                }
            }

            Console.WriteLine(output);
        }
    }
}

namespace sum_of_prime
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int[] input1 = { 1, 2, 3, 4, 5 };
            int input2 = 5;

            int output = SumOfPrimes(input1, input2);
            Console.WriteLine(output);
        }

        static int SumOfPrimes(int[] arr, int size)
        {
            if (size < 0)
                return -2;

            int sum = 0;
            bool hasPrime = false;

            for (int i = 0; i < size; i++)
            {
                if (arr[i] < 0)
                    return -1;

                if (IsPrime(arr[i]))
                {
                    sum += arr[i];
                    hasPrime = true;
                }
            }

            if (!hasPrime)
                return -3;

            return sum;
        }

        static bool IsPrime(int n)
        {
            if (n <= 1)
                return false;

            for (int i = 2; i <= n / 2; i++)
            {
                if (n % i == 0)
                    return false;
            }
            return true;

        }
    }
}

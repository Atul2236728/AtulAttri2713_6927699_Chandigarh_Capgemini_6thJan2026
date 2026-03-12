namespace sum_of_cubes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine(SumOfPrimeCubes(n));
        }

        static int SumOfPrimeCubes(int n)
        {
            if (n < 0)
                return -1;
            if (n > 32676)
                return -2;

            int sum = 0;

            for (int i = 2; i <= n; i++)
            {
                if (IsPrime(i))
                    sum += i * i * i;
            }
            return sum;
        }

        static bool IsPrime(int n)
        {
            if (n <= 1) return false;
            for (int i = 2; i <= n / 2; i++)
                if (n % i == 0) return false;
            return true;
        }
    }
}

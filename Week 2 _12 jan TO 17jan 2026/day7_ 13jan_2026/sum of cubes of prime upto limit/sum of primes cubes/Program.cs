namespace sum_of_primes_cubes
{
    internal class Program
    {
        static bool IsPrime(int num)
        {
            if (num < 2) return false;

            for (int i = 2; i <= num/2; i++)
                if (num % i == 0)
                    return false;

            return true;
        }
        static void Main(string[] args)
        {
            Console.Write("Enter input: ");
            int input = Convert.ToInt32(Console.ReadLine());

            if (input < 0)
            {
                Console.WriteLine("-1");
                return;
            }

            if (input > 32767)
            {
                Console.WriteLine("-2");
                return;
            }

            long sum = 0;

            for (int i = 2; i <= input; i++)
            {
                if (IsPrime(i))
                    sum += (long)i * i * i;
            }

            Console.WriteLine("Sum of cubes of primes: " + sum);
        }
    }
}

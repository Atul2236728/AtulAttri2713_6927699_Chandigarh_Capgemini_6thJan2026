namespace perfect_no_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!");
            Console.WriteLine("enter the no.");
            int n=Convert.ToInt32(Console.ReadLine());
            int output = Perfectno(n);
            Console.WriteLine("output" + output);
        }
        static int Perfectno(int n)
        {
            if (n < 0)
                return -2;
            int sum = 0;
            for (int i = 1; i <= n / 2; i++)
            {
                if (n % i == 0)
                    sum += i;
            }
            if (sum == n && n != 0)
                return 1;
            else
                return -1;
        }
    }
}

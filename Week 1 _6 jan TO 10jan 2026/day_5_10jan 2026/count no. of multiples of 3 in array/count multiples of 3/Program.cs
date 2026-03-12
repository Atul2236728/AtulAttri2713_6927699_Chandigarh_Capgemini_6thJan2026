namespace count_multiples_of_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] arr = { 3, 6, 4, 9, 12 };

            int output = CountMultiplesOf3(arr);
            Console.WriteLine("Output: " + output);
        }

        static int CountMultiplesOf3(int[] arr)
        {
            int count = 0;

            foreach (int x in arr)
            {
                if (x < 0)
                    return -1;

                if (x % 3 == 0)
                    count++;
            }
            return count;
        }
    }
}

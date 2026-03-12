namespace search_ele
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter array size: ");
            int n = Convert.ToInt32(Console.ReadLine());

            if (n < 0)
            {
                Console.WriteLine("-2");
                return;
            }

            int[] arr = new int[n];
            bool hasNegative = false;

            Console.WriteLine("Enter array elements:");
            for (int i = 0; i < n; i++)
            {
                arr[i] = Convert.ToInt32(Console.ReadLine());
                if (arr[i] < 0)
                    hasNegative = true;
            }

            if (hasNegative)
            {
                Console.WriteLine("-1");
                return;
            }

            Console.Write("Enter element to search: ");
            int key = Convert.ToInt32(Console.ReadLine());

            foreach (int x in arr)
            {
                if (x == key)
                {
                    Console.WriteLine("1");
                    return;
                }
            }

            Console.WriteLine("-3");
        }
    }
}

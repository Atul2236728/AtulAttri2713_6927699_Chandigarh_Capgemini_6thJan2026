namespace Product_of_Max_min
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Enter the size of the array:");
            int size = int.Parse(Console.ReadLine());

            if (size < 0)
            {
                Console.WriteLine("-2");
                return;
            }

            int[] arr = new int[size];

            Console.WriteLine("Enter the array elements:");
            for (int i = 0; i < size; i++)
            {
                arr[i] = int.Parse(Console.ReadLine());
                if (arr[i] < 0)
                {
                    Console.WriteLine("-1");
                    return;
                }
            }

            int max = arr[0];
            int min = arr[0];

            for (int i = 1; i < size; i++)
            {
                if (arr[i] > max)
                    max = arr[i];
                if (arr[i] < min)
                    min = arr[i];
            }

            int output = max * min;
            Console.WriteLine("Output:");
            Console.WriteLine(output);
        }
    }
}

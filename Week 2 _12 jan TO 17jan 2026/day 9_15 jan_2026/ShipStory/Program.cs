namespace ShipStory
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Enter the size of the arrays:");
            int size = int.Parse(Console.ReadLine());

            if (size < 0)
            {
                Console.WriteLine("-2");
                return;
            }

            int[] arr1 = new int[size];
            int[] arr2 = new int[size];

            Console.WriteLine("Enter elements of first array:");
            for (int i = 0; i < size; i++)
            {
                arr1[i] = int.Parse(Console.ReadLine());
                if (arr1[i] < 0)
                {
                    Console.WriteLine("-1");
                    return;
                }
            }

            Console.WriteLine("Enter elements of second array:");
            for (int i = 0; i < size; i++)
            {
                arr2[i] = int.Parse(Console.ReadLine());
                if (arr2[i] < 0)
                {
                    Console.WriteLine("-1");
                    return;
                }
            }

            int[] output = new int[size];

            for (int i = 0; i < size; i++)
            {
                output[i] = arr1[i] + arr2[size - 1 - i];
            }

            Console.WriteLine("Output:");
            foreach (int val in output)
                Console.Write(val + " ");
        }
    }
}

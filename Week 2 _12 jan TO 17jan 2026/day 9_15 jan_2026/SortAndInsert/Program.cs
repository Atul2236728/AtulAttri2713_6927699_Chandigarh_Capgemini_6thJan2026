namespace SortAndInsert
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

            Console.WriteLine("Enter the element to insert:");
            int element = int.Parse(Console.ReadLine());

            if (element < 0)
            {
                Console.WriteLine("-1");
                return;
            }

            Array.Sort(arr);

            int[] output = new int[size + 1];
            int index = 0;
            bool inserted = false;

            for (int i = 0; i < size; i++)
            {
                if (!inserted && element < arr[i])
                {
                    output[index++] = element;
                    inserted = true;
                }
                output[index++] = arr[i];
            }

            if (!inserted)
                output[index] = element;

            Console.WriteLine("Output:");
            foreach (int val in output)
                Console.Write(val + " ");
        }
    }
}

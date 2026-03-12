namespace remove_and_sort
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter array size:");
            int size = Convert.ToInt32(Console.ReadLine());
            if (size < 0)
            {
                int[] output = { -1 };
                Console.WriteLine("Output:");
                Console.WriteLine(output[0]);
                return;
            }
            int[] inputArray = new int[size];
            Console.WriteLine("Enter array elements:");

            for (int i = 0; i < size; i++)
            {
                inputArray[i] = Convert.ToInt32(Console.ReadLine());
            }
            int positiveCount = 0;
            for (int i = 0; i < size; i++)
            {
                if (inputArray[i] >= 0)
                {
                    positiveCount++;
                }
            }
            int[] outputArray = new int[positiveCount];

            int index = 0;
            for (int i = 0; i < size; i++)
            {
                if (inputArray[i] >= 0)
                {
                    outputArray[index] = inputArray[i];
                    index++;
                }
            }
            Array.Sort(outputArray);

            Console.WriteLine("Output array after removing negatives and sorting:");
            foreach (int value in outputArray)
            {
                Console.Write(value + " ");
            }
        }
    }
}

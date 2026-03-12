namespace remove_neg_values_from_array_and_sort
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] arr = { 20, -10, 4, 78 };
            int size = arr.Length;

            if (size < 0)
            {
                Console.WriteLine(-1);
                return;
            }

            int[] temp = new int[size];
            int k = 0;

            for (int i = 0; i < size; i++)
            {
                if (arr[i] >= 0)
                    temp[k++] = arr[i];
            }

            Array.Sort(temp, 0, k);

            for (int i = 0; i < k; i++)
                Console.Write(temp[i] + " ");
        }
    }
}

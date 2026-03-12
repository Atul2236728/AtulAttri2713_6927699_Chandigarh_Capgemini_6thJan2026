namespace remove_repeated_ele
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] arr = { 1, 2, 2, 3, 4, 4 };
            int[] output = RemoveDuplicates(arr);

            foreach (int x in output)
                Console.Write(x + " ");
        }

        static int[] RemoveDuplicates(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] < 0)
                    return new int[] { -1 };
            }

            int[] temp = new int[arr.Length];
            int k = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                bool found = false;
                for (int j = 0; j < k; j++)
                {
                    if (arr[i] == temp[j])
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    temp[k++] = arr[i];
            }

            int[] result = new int[k];
            for (int i = 0; i < k; i++)
                result[i] = temp[i];

            return result;
        }
    }
}

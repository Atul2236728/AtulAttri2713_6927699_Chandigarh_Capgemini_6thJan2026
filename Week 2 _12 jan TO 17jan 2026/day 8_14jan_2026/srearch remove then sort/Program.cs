namespace srearch_remove_then_sort
{
    class ArrayBase
    {
        public virtual int[] Process(int[] arr, int size, int search)
        {
            return null;
        }
    }
    class RemoveAndSort : ArrayBase
    {
        public override int[] Process(int[] arr, int size, int search)
        {
            if (size < 0)
                return new int[] { -2 };

            foreach (int x in arr)
            {
                if (x < 0)
                    return new int[] { -1 };
            }

            int index = Array.IndexOf(arr, search);
            if (index == -1)
                return new int[] { -3 };

            int[] result = new int[size - 1];
            int k = 0;

            for (int i = 0; i < size; i++)
            {
                if (i != index)
                    result[k++] = arr[i];
            }

            Array.Sort(result);
            return result;
        }
    }
    class Program
    {
        static void Main()
        {
            int size = Convert.ToInt32(Console.ReadLine());
            int[] arr = new int[size];

            for (int i = 0; i < size; i++)
                arr[i] = Convert.ToInt32(Console.ReadLine());

            int search = Convert.ToInt32(Console.ReadLine());

            ArrayBase obj = new RemoveAndSort();
            int[] output = obj.Process(arr, size, search);

            foreach (int val in output)
                Console.Write(val + " ");
        }
    }

}

namespace sorting_and_adding
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] arr1 = { 1, 2, 3, 4, 5 };
            int[] arr2 = { 9, 8, 7, 6, 5 };

            int n = arr1.Length;
            int[] output = new int[n];
            if (n < 0)
            {
                output[0] = -2;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (arr1[i] < 0 || arr2[i] < 0)
                    {
                        output[0] = -1;
                        Console.WriteLine(output[0]);
                        return;
                    }
                }

                Array.Sort(arr1);          
                Array.Sort(arr2);
                Array.Reverse(arr2);
               

                for (int i = 0; i < n; i++)
                {
                    output[i] = arr1[i] * arr2[n - 1 - i];
                }
            }

            for (int i = 0; i < n; i++)
            {
                Console.Write(output[i] + " ");
            }
        }
    }
    }


namespace multiply
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] arr = { 1, -2, 100, 40 };
            int n = arr.Length;
            int output;

            if (n < 0)
            {
                output = -2;
            }
            else
            {
                output = 1;
                for (int i = 0; i < n; i++)
                {
                    if (arr[i] > 0)
                        output *= arr[i];
                }
            }

            Console.WriteLine(output);
        }
    }
}

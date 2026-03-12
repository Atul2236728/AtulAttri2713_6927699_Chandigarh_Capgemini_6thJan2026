namespace searching
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] arr = { 10, 20, 30, 40 };
            int key = 30;
            int n = arr.Length;
            int output = 1; 

            if (n < 0)
            {
                output = -2;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (arr[i] < 0)
                    {
                        output = -1;
                        break;
                    }
                    if (arr[i] == key)
                    {
                        output = i; 
                        break;
                    }
                }
            }

            Console.WriteLine(output);
        }
    }
}
 


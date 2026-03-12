namespace repeating_element
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] input = { 2, 2, 2, 3, 3, 3, 3, 4 };
            int n = input.Length;

            int maxCount = 0;

            for (int i = 0; i < n; i++)
            {
                int count = 0;
                for (int j = 0; j < n; j++)
                    if (input[i] == input[j])
                        count++;

                if (count > maxCount)
                    maxCount = count;
            }

            for (int i = 0; i < n; i++)
            {
                bool printed = false;
                for (int k = 0; k < i; k++)
                    if (input[i] == input[k])
                        printed = true;

                if (printed) continue;

                int count = 0;
                for (int j = 0; j < n; j++)
                    if (input[i] == input[j])
                        count++;

                if (count == maxCount)
                    Console.Write(input[i] + " ");
            }
        }
    }
}

namespace replacing_first_wiht_thge_last
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] input1 = { 12, 34, 56, 17, 2 };
            int input2 = 5;
            int[] output1 = new int[input2];


            for (int i = 0; i < input2; i++)
                output1[i] = input1[i];


            if (input2 < 0)
            {
                output1[0] = -2;
            }
            else if (input2 % 2 == 0)
            {
                output1[0] = -3;
            }
            else
            {
                for (int i = 0; i < input2; i++)
                {
                    if (input1[i] < 0)
                    {
                        output1[0] = -1;
                        PrintArray(output1);
                        return;
                    }
                }

                int mid = input2 / 2;

                for (int i = 0; i < mid; i++)
                {
                    int temp = output1[i];
                    output1[i] = output1[input2 - 1 - i];
                    output1[input2 - 1 - i] = temp;
                }
            }

            PrintArray(output1);
        }

        static void PrintArray(int[] arr)
        {
            Console.Write("Output1 = { ");
            foreach (int x in arr)
                Console.Write(x + " ");
            Console.WriteLine("}");
        }
    }
}
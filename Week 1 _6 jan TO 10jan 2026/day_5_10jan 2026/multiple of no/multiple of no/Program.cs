namespace multiple_of_no_
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int[] input1 = { 1, 2, 3, 4, 5, 6 };
            int output;
            int count = 0;
            int input2 = input1.Length;
            if (input2 < 0)
            {
                output = -2;

            } else
            {
                for (int i = 0; i < input2; i++)
                {


                    if (input1[i] < 0)
                    {
                        output = -1;
                        break;
                    }
                    if (input1[i] % 3 == 0) {
                        count++;

                    }
                }
            }
            Console.WriteLine(count);
        }
        
    }
}

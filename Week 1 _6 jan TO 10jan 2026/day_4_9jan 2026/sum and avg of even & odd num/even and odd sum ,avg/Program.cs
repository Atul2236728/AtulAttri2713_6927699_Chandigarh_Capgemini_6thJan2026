using System.ComponentModel.DataAnnotations;

namespace even_and_odd_sum__avg
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double output1; 
                int evensum = 0;
            int oddsum = 0;
            int[] array = { 1, 2, 3, 4, 5, 6 };
            int size = array.Length;
            if (size < 0)
            {
                output1 = -2;
                Console.WriteLine(output1);
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    if (array[i] % 2 == 0)
                    {
                        evensum = evensum + array[i];
                    }
                    else
                    {
                        oddsum = oddsum + array[i];
                    }
                    
                }
                output1 = (oddsum + evensum) / 2.0;
                Console.WriteLine(output1);
            }
        }
    }
}

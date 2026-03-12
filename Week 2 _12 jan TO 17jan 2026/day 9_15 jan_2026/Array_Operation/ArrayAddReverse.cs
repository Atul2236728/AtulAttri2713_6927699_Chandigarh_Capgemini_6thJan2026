using System;
using System.Collections.Generic;
using System.Text;

namespace Array_Operation
{
    internal class ArrayAddReverse
    {
        static void Main()
        {
            Console.WriteLine("Enter the size ");
            int input3 = int.Parse(Console.ReadLine());
            int[] input1 = new int[input3];
            int[] input2 = new int[input3];
            int[] output;

            if (input3 < 0)
            {
                Console.WriteLine("-2");
                return;
            }
            Console.WriteLine("Enter the elements");
            for (int i = 0; i < input3; i++)
                input1[i] = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the 2nd Array elements ");
            for (int i = 0; i < input3; i++)
                input2[i] = int.Parse(Console.ReadLine());

            for (int i = 0; i < input3; i++)
            {
                if (input1[i] < 0 || input2[i] < 0)
                {
                    Console.WriteLine("-1");
                    return;
                }
            }

            output = new int[input3];

            for (int i = 0; i < input3; i++)
                output[i] = input1[i] + input2[input3 - 1 - i];

            foreach (int val in output)
                Console.Write(val + " ");
        }
    }
}

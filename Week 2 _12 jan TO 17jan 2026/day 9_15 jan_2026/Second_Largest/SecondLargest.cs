using System;
using System.Collections.Generic;
using System.Text;

namespace Second_Largest
{
    internal class SecondLargest
    {
        static void Main()
        {
            Console.WriteLine("Enter the Size");
            int input3 = int.Parse(Console.ReadLine());
            int[] input1 = new int[input3];

            if (input3 < 0)
            {
                Console.WriteLine("-2");
                return;
            }
            Console.WriteLine("Enter the Elements");
            for (int i = 0; i < input3; i++)
            {
                input1[i] = int.Parse(Console.ReadLine());
                if (input1[i] < 0)
                {
                    Console.WriteLine("-1");
                    return;
                }
            }

            int largest = int.MinValue;
            int secondLargest = int.MinValue;

            for (int i = 0; i < input3; i++)
            {
                if (input1[i] > largest)
                {
                    secondLargest = largest;
                    largest = input1[i];
                }
                else if (input1[i] > secondLargest && input1[i] != largest)
                {
                    secondLargest = input1[i];
                }
            }

            Console.WriteLine(secondLargest);
        }
    }
}

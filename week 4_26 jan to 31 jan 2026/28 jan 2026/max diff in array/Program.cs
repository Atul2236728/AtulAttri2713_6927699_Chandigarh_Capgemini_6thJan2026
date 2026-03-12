using System;
using System.Collections.Generic;

class UserProgramCode
{
    public static int diffIntArray(int[] input1)
    {
        int n = input1.Length;

        // Rule 2
        if (n <= 1 || n > 10)
            return -2;

        HashSet<int> set = new HashSet<int>();

        // Rule 1 & 3
        foreach (int num in input1)
        {
            if (num < 0)
                return -1;

            if (!set.Add(num))
                return -3;
        }

        int min = input1[0];
        int maxDiff = 0;

        for (int i = 1; i < n; i++)
        {
            if (input1[i] > min)
                maxDiff = Math.Max(maxDiff, input1[i] - min);
            else
                min = input1[i];
        }

        return maxDiff;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("enter the size ");
        int n = int.Parse(Console.ReadLine());

        int[] input1 = new int[n];
        for (int i = 0; i < n; i++)
        {
            input1[i] = int.Parse(Console.ReadLine());
        }
        Console.WriteLine("output ");
        int result = UserProgramCode.diffIntArray(input1);
        Console.WriteLine(result);
    }
}

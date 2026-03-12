using System;
using System.Collections.Generic;

class UserProgramCode
{
    public static List<int> GetElements(List<int> input1, int input2)
    {
        List<int> result = new List<int>();

        foreach (int num in input1)
        {
            if (num < input2)
                result.Add(num);
        }

        if (result.Count == 0)
        {
            result.Add(-1);
            return result;
        }

        result.Sort();
        result.Reverse();

        return result;
    }
}

class ListTheElementsA
{
    static void Main()
    {
        Console.WriteLine("enter the size ");
        int n = int.Parse(Console.ReadLine());

        List<int> input1 = new List<int>();
        for (int i = 0; i < n; i++)
        {
            input1.Add(int.Parse(Console.ReadLine()));
        }
        Console.WriteLine("enter the limiting ele ");
        int input2 = int.Parse(Console.ReadLine());

        List<int> output = UserProgramCode.GetElements(input1, input2);

        if (output.Count == 1 && output[0] == -1)
        {
            Console.WriteLine("No element found");
        }
        else
        {
            foreach (int val in output)
            {
                Console.Write(val + " ");
            }
        }
    }
}


using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.Write("Enter number of elements: ");
        int n = int.Parse(Console.ReadLine());

        string[] arr = new string[n];
        bool valid = true;

        for (int i = 0; i < n; i++)
        {
            Console.Write($"Enter element {i + 1}: ");
            arr[i] = Console.ReadLine();

            if (!Regex.IsMatch(arr[i], @"^\d+(\.\d+)?$"))
                valid = false;
        }

        Console.WriteLine(valid ? 1 : -1);
    }
}

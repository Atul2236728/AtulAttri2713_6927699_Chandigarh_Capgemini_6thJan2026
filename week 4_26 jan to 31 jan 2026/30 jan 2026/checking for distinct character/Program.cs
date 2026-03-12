using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.Write("Enter string: ");
        string input = Console.ReadLine();

        HashSet<char> set = new HashSet<char>(input);

        Console.WriteLine(set.Count == input.Length ? 1 : -1);
    }
}

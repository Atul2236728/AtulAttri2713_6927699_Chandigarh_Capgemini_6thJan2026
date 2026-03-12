using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter the string: ");
        string s = Console.ReadLine();

        int maxDeletions = s.Length / 2;

        Console.WriteLine("Maximum deletions possible: " + maxDeletions);
    }
}

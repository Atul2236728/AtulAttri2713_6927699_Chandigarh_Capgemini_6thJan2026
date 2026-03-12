using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.Write("Enter string: ");
        string input = Console.ReadLine();

        MatchCollection matches =
            Regex.Matches(input, @"(.)\1{2,}");

        Console.WriteLine(matches.Count);
    }
}

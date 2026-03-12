using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {

        Console.WriteLine("enter the date  ");
        string text = Console.ReadLine();

        string pattern = @"\b\d{2}/\d{2}/\d{4}\b";

        foreach (Match m in Regex.Matches(text, pattern))
        {
            Console.WriteLine(m.Value);
        }
    }
}

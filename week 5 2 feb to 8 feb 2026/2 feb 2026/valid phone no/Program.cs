using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        string text = Console.ReadLine();

        string pattern = @"\b\d{10}\b";

        MatchCollection matches = Regex.Matches(text, pattern);

        foreach (Match m in matches)
        {
            Console.WriteLine(m.Value);
        }
    }
}

using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        string time = Console.ReadLine();

        if (Regex.IsMatch(time, @"^(0[1-9]|1[0-2]):[0-5][0-9]\s?(am|pm)$",
                          RegexOptions.IgnoreCase))
            Console.WriteLine(1);
        else
            Console.WriteLine(-1);
    }
}

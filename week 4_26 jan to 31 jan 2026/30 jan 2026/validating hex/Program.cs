using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.Write("Enter hex color code: ");
        string color = Console.ReadLine();

        bool valid = Regex.IsMatch(color, @"^#[0-9A-Fa-f]{6}$");

        Console.WriteLine(valid ? "Valid Hex Color" : "Invalid Hex Color");
    }
}

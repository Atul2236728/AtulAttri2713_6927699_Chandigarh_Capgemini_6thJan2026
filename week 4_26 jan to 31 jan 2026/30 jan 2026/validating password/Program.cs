using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        string pattern =
            @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@#_])[A-Za-z][A-Za-z\d@#_]{6,}[A-Za-z\d]$";

        bool valid = Regex.IsMatch(password, pattern);

        Console.WriteLine(valid ? 1 : -1);
    }
}

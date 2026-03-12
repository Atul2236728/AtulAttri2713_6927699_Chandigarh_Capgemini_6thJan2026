using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.WriteLine("enter the password ");
        string password = Console.ReadLine();

        string pattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$";

        Console.WriteLine(
            Regex.IsMatch(password, pattern) ? "Strong" : "Weak"
        );
    }
}

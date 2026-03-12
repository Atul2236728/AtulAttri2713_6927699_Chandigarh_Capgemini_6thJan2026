using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.WriteLine("enter the email");
        string email = Console.ReadLine();

        string pattern = @"^[A-Za-z0-9._]+@gmail\.com$";

        if (Regex.IsMatch(email, pattern))
            Console.WriteLine("Valid");
        else
            Console.WriteLine("Invalid");
    }
}

using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter a string: ");
        string s = Console.ReadLine();

        string result = "";

        for (int i = 0; i < s.Length; i++)
        {
            if (i % 2 == 0)   // Keep even index characters
            {
                result += s[i];
            }
        }

        Console.WriteLine("Modified String: " + result);
    }
}

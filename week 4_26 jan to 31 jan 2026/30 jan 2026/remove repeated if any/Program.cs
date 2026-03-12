using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter string: ");
        string input = Console.ReadLine();

        string output = "";

        foreach (char c in input)
        {
            if (!output.Contains(c))
                output += c;
        }

        Console.WriteLine("Output: " + output);
    }
}

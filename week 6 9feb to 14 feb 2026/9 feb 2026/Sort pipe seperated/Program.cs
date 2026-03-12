using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter pipe separated words: ");
        string input = Console.ReadLine();

        string[] words = input.Split('|');
        Array.Sort(words);

        Console.WriteLine(string.Join("|", words));
    }
}

using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter string: ");
        string str = Console.ReadLine();

        Console.Write("Enter word to remove: ");
        string word = Console.ReadLine();

        int index = str.LastIndexOf(word);

        if (index != -1)
            str = str.Remove(index, word.Length);

        Console.WriteLine("Result:");
        Console.WriteLine(str);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.Write("Enter number of words: ");
        int n = int.Parse(Console.ReadLine());

        List<string> words = new List<string>();

        for (int i = 0; i < n; i++)
        {
            Console.Write("Enter word: ");
            words.Add(Console.ReadLine());
        }

        var groups = words.GroupBy(w => String.Concat(w.OrderBy(c => c)));

        List<string> result = new List<string>();

        foreach (var group in groups)
        {
            if (group.Count() == 1)
                result.Add(group.First());
        }

        Console.WriteLine("Unique words:");
        Console.WriteLine(string.Join(", ", result));
    }
}

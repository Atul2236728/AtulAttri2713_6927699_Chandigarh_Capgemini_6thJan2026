using System;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.Write("Enter comma separated words: ");
        string[] words = Console.ReadLine().Split(',');

        for (int i = 0; i < words.Length; i++)
            words[i] = words[i].Trim();

        string sortedFirst = String.Concat(words[0].OrderBy(c => c));

        bool allAnagrams = true;

        for (int i = 1; i < words.Length; i++)
        {
            string sortedWord = String.Concat(words[i].OrderBy(c => c));
            if (sortedWord != sortedFirst)
            {
                allAnagrams = false;
                break;
            }
        }

        Console.WriteLine(allAnagrams);
    }
}

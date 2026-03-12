using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter string: ");
        string s = Console.ReadLine();

        int deletions = 0;

        for (int i = 0; i < s.Length - 1; i++)
        {
            if (IsVowel(s[i]) && IsVowel(s[i + 1]))
            {
                deletions++;
                i++;
            }
        }

        Console.WriteLine("Deletions: " + deletions);
    }

    static bool IsVowel(char c)
    {
        return "aeiouAEIOU".IndexOf(c) != -1;
    }
}

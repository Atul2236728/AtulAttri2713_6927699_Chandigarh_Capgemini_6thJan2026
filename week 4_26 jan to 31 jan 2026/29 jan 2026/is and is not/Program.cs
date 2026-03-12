using System;

class Program1
{
    public static string negativeString(string input)
    {
        string[] words = input.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i] == "is")
            {
                words[i] = "is not";
            }
        }
        return string.Join(" ", words);
    }
}

class Program
{
    static void Main()
    {
        string input = Console.ReadLine();
        Console.WriteLine(Program1.negativeString(input));
    }
}

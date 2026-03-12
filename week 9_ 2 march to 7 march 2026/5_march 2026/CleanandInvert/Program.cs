using System;

public class Program
{
    public static string CleanseAndInvert(string input)
    {
        // Rule 1
        if (input == null || input.Length < 6)
        {
            return "";
        }

        // Rule 2
        foreach (char c in input)
        {
            if (!char.IsLetter(c))
            {
                return "";
            }
        }

        // Step 1: convert to lowercase
        input = input.ToLower();

        string filtered = "";

        // Step 2: remove even ASCII characters
        foreach (char c in input)
        {
            if ((int)c % 2 != 0)
            {
                filtered += c;
            }
        }

        // Step 3: reverse string
        char[] arr = filtered.ToCharArray();
        Array.Reverse(arr);
        string reversed = new string(arr);

        // Step 4: uppercase even index
        char[] result = reversed.ToCharArray();

        for (int i = 0; i < result.Length; i++)
        {
            if (i % 2 == 0)
            {
                result[i] = Char.ToUpper(result[i]);
            }
        }

        return new string(result);
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("Enter the word");
        string input = Console.ReadLine();

        string output = CleanseAndInvert(input);

        if (output == "")
        {
            Console.WriteLine("Invalid Input");
        }
        else
        {
            Console.WriteLine("The generated key is - " + output);
        }
    }
}
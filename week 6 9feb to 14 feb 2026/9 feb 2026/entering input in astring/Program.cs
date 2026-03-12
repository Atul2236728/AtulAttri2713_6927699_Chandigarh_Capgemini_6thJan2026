using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter the original string: ");
        string original = Console.ReadLine();

        Console.Write("Enter the character to insert: ");
        char ch = Convert.ToChar(Console.ReadLine());

        Console.Write("Enter the position (starting from 0): ");
        int position = Convert.ToInt32(Console.ReadLine());

        if (position < 0 || position > original.Length)
        {
            Console.WriteLine("Invalid position!");
            return;
        }

        string result = original.Substring(0, position)
                        + ch
                        + original.Substring(position);

        Console.WriteLine("Modified String: " + result);
    }
}

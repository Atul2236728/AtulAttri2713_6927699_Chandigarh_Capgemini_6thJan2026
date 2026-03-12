using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter string: ");
        string str = Console.ReadLine();

        Console.Write("Enter character to replace: ");
        char oldChar = char.Parse(Console.ReadLine());

        Console.Write("Enter new character: ");
        char newChar = char.Parse(Console.ReadLine());

        int index = str.IndexOf(oldChar);

        if (index != -1)
        {
            char[] arr = str.ToCharArray();
            arr[index] = newChar;
            str = new string(arr);
        }

        Console.WriteLine("Updated string:");
        Console.WriteLine(str);
    }
}

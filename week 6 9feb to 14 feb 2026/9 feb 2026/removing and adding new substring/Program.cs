using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter string: ");
        string str = Console.ReadLine();

        Console.Write("Enter substring to remove: ");
        string remove = Console.ReadLine();

        Console.Write("Enter substring to insert: ");
        string insert = Console.ReadLine();

        int index = str.IndexOf(remove);

        if (index != -1)
        {
            str = str.Remove(index, remove.Length);
            str = str.Insert(index, insert);
        }

        Console.WriteLine("Updated string:");
        Console.WriteLine(str);
    }
}

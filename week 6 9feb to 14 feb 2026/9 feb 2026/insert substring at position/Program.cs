using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter original string: ");
        string str = Console.ReadLine();

        Console.Write("Enter substring to insert: ");
        string insert = Console.ReadLine();

        Console.Write("Enter position: ");
        int pos = int.Parse(Console.ReadLine());

        string result = str.Insert(pos, insert);

        Console.WriteLine("Result:");
        Console.WriteLine(result);
    }
}

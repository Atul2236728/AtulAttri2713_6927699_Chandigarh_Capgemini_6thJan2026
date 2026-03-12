using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter input1: ");
        string input1 = Console.ReadLine();

        Console.Write("Enter input2: ");
        string input2 = Console.ReadLine();

        Console.Write("Enter input3: ");
        string input3 = Console.ReadLine();

        int i2 = input1.IndexOf(input2);
        int i3 = input1.IndexOf(input3);

        if (i2 != -1 && i3 != -1 && i3 > i2)
            Console.WriteLine(1);
        else
            Console.WriteLine(-1);
    }
}

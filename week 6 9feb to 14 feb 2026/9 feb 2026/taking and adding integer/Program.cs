using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter a positive integer: ");
        string number = Console.ReadLine();

        int sum = 0;

        foreach (char digit in number)
        {
            sum += digit - '0';
        }

        Console.WriteLine("Sum of digits: " + sum);
    }
}

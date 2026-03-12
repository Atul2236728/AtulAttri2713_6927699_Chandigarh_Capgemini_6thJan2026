using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter a positive integer: ");
        int number = int.Parse(Console.ReadLine());

        double root = Math.Sqrt(number);
        int nearest = (int)Math.Round(root);

        int result = nearest * nearest;

        Console.WriteLine("Closest perfect square: " + result);
    }
}

using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter a positive integer (1 to 7 digits): ");
        int number = Convert.ToInt32(Console.ReadLine());

        if (number < 0)
        {
            Console.WriteLine("Please enter a positive number.");
            return;
        }

        double root = Math.Sqrt(number);

        int lower = (int)Math.Floor(root);
        int upper = (int)Math.Ceiling(root);

        int lowerSquare = lower * lower;
        int upperSquare = upper * upper;

        // Compare which square is closer
        if (number - lowerSquare <= upperSquare - number)
            Console.WriteLine("Closest perfect square: " + lowerSquare);
        else
            Console.WriteLine("Closest perfect square: " + upperSquare);
    }
}

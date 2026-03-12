using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter a positive integer: ");
        int number = int.Parse(Console.ReadLine());

        int sum = 0;

        while (number > 0)
        {
            sum += number % 10;
            number /= 10;
        }

        Console.WriteLine("Sum of digits: " + sum);
    }
}

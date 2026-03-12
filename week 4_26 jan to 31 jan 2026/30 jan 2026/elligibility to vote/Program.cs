using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter age: ");
        int age = int.Parse(Console.ReadLine());

        Console.WriteLine(age >= 18 ? "Eligible to Vote" : "Not Eligible to Vote");
    }
}

using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter alphanumeric string: ");
        string str = Console.ReadLine();

        string left = "";
        string right = "";

        foreach (char c in str)
        {
            if (char.IsDigit(c))
                left += c;
            else
                right += c;
        }

        Console.WriteLine("Left (digits): " + left);
        Console.WriteLine("Right (letters): " + right);
    }
}

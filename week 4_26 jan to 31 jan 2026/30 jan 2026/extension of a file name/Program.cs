using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("enter the file format");
        string fileName = Console.ReadLine();

        int index = fileName.LastIndexOf('.');
        string extension = fileName.Substring(index + 1);

        Console.WriteLine(extension);
    }
}

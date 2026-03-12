using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("enter the no. of strings ");
        int n = int.Parse(Console.ReadLine());
        string[] arr = new string[n];

        for (int i = 0; i < n; i++)
        {
            arr[i] = Console.ReadLine();
        }

        string designation = Console.ReadLine();

        string[] result = UserProgramCode.getEmployee(arr, designation);

        if (result.Length == 1)
        {
            Console.WriteLine(result[0]);
        }
        else
        {
            Console.WriteLine(string.Join(" ", result));
        }
    }
}

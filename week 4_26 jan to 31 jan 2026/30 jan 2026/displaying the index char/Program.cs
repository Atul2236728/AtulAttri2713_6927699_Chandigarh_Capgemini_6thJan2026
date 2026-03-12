using System;

class Program
{
    static void Main()
    {
        int k = int.Parse(Console.ReadLine());
        string[] arr = new string[k];

        for (int i = 0; i < k; i++)
        {
            arr[i] = Console.ReadLine();
        }

        int n = int.Parse(Console.ReadLine());

        string result = UserProgramCode.formString(arr, n);
        Console.WriteLine(result);
    }
}

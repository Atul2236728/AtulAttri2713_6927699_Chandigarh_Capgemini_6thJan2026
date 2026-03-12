using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter N: ");
        int N = int.Parse(Console.ReadLine());

        Console.Write("Enter array elements separated by space: ");
        string[] input = Console.ReadLine().Split();

        int[] arr = new int[N];
        for (int i = 0; i < N; i++)
            arr[i] = int.Parse(input[i]);

        int count = 0;

        for (int i = 0; i < N - 1; i++)
        {
            if ((arr[i] + arr[i + 1]) % N == 0)
                count++;
        }

        Console.WriteLine("Total couples divisible by N: " + count);
    }
}

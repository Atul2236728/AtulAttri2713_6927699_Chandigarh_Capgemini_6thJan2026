using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter array size (N): ");
        int N = Convert.ToInt32(Console.ReadLine());

        int[] arr = new int[N];

        Console.WriteLine("Enter array elements:");
        for (int i = 0; i < N; i++)
        {
            arr[i] = Convert.ToInt32(Console.ReadLine());
        }

        int count = 0;

        Console.WriteLine("\nAll Couples Formed:");

        for (int i = 0; i < N - 1; i++)
        {
            int first = arr[i];
            int second = arr[i + 1];
            int sum = first + second;

            Console.WriteLine("(" + first + ", " + second + ") -> Sum = " + sum);

            if (sum % N == 0)
            {
                count++;
            }
        }

        Console.WriteLine("\nCouples Whose Sum Is Divisible By " + N + ":");

        for (int i = 0; i < N - 1; i++)
        {
            int sum = arr[i] + arr[i + 1];

            if (sum % N == 0)
            {
                Console.WriteLine("(" + arr[i] + ", " + arr[i + 1] + ")");
            }
        }

        Console.WriteLine("\nTotal Valid Couples: " + count);
    }
}

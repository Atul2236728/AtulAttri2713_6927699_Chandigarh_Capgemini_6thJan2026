namespace Plot_Password
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Enter the size of the array:");
            int size = int.Parse(Console.ReadLine());

            if (size < 0)
            {
                Console.WriteLine("-2");
                return;
            }

            int[] arr = new int[size];

            Console.WriteLine("Enter the plot numbers:");
            for (int i = 0; i < size; i++)
            {
                arr[i] = int.Parse(Console.ReadLine());
                if (arr[i] < 0)
                {
                    Console.WriteLine("-1");
                    return;
                }
            }

            int sumOdd = 0, sumEven = 0;

            for (int i = 0; i < size; i++)
            {
                if (arr[i] % 2 == 0)
                    sumEven += arr[i];
                else
                    sumOdd += arr[i];
            }

            int password = (sumOdd + sumEven) / 2;

            Console.WriteLine("Output:");
            Console.WriteLine(password);
        }
    }
}

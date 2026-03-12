namespace mahirlandmath
{

    using System;

    class Program
    {
        static void Main()
        {
            Console.WriteLine("enter target");
            int target = int.Parse(Console.ReadLine());

            // Safe upper bound
            int limit = target * 3 + 10;

            // Stores minimum steps to reach each number
            int[] steps = new int[limit + 1];

            // Stores numbers in the order they are reached
            int[] order = new int[limit + 1];

            // Initialize all steps as -1 (not reached)
            for (int i = 0; i <= limit; i++)
            {
                steps[i] = -1;
            }

            // Starting point
            steps[10] = 0;
            order[0] = 10;

            int index = 0;
            int size = 1;

            // Process numbers in discovery order
            while (index < size)
            {
                int current = order[index];
                int nextSteps = steps[current] + 1;
                index++;

                // +2 operation
                int a = current + 2;
                if (a <= limit && steps[a] == -1)
                {
                    steps[a] = nextSteps;
                    order[size++] = a;
                }

                // -1 operation
                int b = current - 1;
                if (b >= 0 && steps[b] == -1)
                {
                    steps[b] = nextSteps;
                    order[size++] = b;
                }

                // *3 operation
                int c = current * 3;
                if (c <= limit && steps[c] == -1)
                {
                    steps[c] = nextSteps;
                    order[size++] = c;
                }
            }

            // Final answer
            Console.WriteLine(steps[target]);
        }
    }
}


namespace Total_marks
{
    using System;

    class Program
    {
        static void Main()
        {
            int X = int.Parse(Console.ReadLine());
            int Y = int.Parse(Console.ReadLine());
            int N1 = int.Parse(Console.ReadLine());
            int N2 = int.Parse(Console.ReadLine());
            int M = int.Parse(Console.ReadLine());

            bool isValid = false;
            int type1Correct = 0;
            int type2Correct = 0;

            // Try maximum Type 1 questions first
            for (int i = N1; i >= 0; i--)
            {
                int remainingMarks = M - (i * X);

                if (remainingMarks < 0)
                    continue;

                if (remainingMarks % Y == 0)
                {
                    int j = remainingMarks / Y;

                    if (j <= N2)
                    {
                        type1Correct = i;
                        type2Correct = j;
                        isValid = true;
                        break;
                    }
                }
            }

            if (isValid)
            {
                Console.WriteLine("Valid");
                Console.WriteLine(type1Correct);
                Console.WriteLine(type2Correct);
            }
            else
            {
                Console.WriteLine("Invalid");
            }
        }
    }

}

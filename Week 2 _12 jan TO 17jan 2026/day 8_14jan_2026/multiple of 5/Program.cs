namespace multiple_of_5
{
    using System;

    class LogicBase
    {
        public virtual int Calculate(int input1)
        {
            return 0;
        }
    }
    class AvgDivBy5 : LogicBase
    {
        public override int Calculate(int input1)
        {
            if (input1 < 0)
                return -1;

            int sum = 0, count = 0;

            for (int i = 1; i <= input1; i++)
            {
                if (i % 5 == 0)
                {
                    sum += i;
                    count++;
                }
            }

            return count == 0 ? 0 : sum / count;
        }
    }
    class Program
    {
        static void Main()
        {
            int input = Convert.ToInt32(Console.ReadLine());
            LogicBase obj = new AvgDivBy5();
            Console.WriteLine(obj.Calculate(input));
        }
    }


}

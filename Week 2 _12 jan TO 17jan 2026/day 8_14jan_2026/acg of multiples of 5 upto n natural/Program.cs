namespace acg_of_multiples_of_5_upto_n_natural
{
    using System;

    class topclass
    {
        public virtual int Calculate(int input1)
        {
            return 0;
        }
    }
    class AvgMultiplesOf5 : topclass
    {
        public override int Calculate(int input1)
        {
            if (input1 < 0)
                return -1;
            if (input1 > 500)
                return -2;

            int sum = 0, count = 0;

            for (int i = 5; i <= input1; i += 5)
            {
                sum += i;
                count++;
            }

            return count == 0 ? 0 : sum / count;
        }
    }
    class Program
    {
        static void Main()
        {
            int input = Convert.ToInt32(Console.ReadLine());
            topclass obj = new AvgMultiplesOf5();
            Console.WriteLine(obj.Calculate(input));
        }
    }

}

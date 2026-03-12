namespace sum_of_digits_of_odd_no_
{
    using System;

    class baseclass
    {
        public virtual int Calculate(int input1)
        {
            return 0;
        }
    }
    class SumOddSquares : baseclass
    {
        public override int Calculate(int input1)
        {
            if (input1 < 0)
                return -1;

            int sum = 0;

            while (input1 > 0)
            {
                int digit = input1 % 10;
                if (digit % 2 != 0)
                    sum += digit * digit;

                input1 /= 10;
            }
            return sum;
        }
    }
    class Program
    {
        static void Main()
        {
            Console.WriteLine("enter the element");
            int input = Convert.ToInt32(Console.ReadLine());
            baseclass obj = new SumOddSquares();
            Console.WriteLine(obj.Calculate(input));
        }
    }

}

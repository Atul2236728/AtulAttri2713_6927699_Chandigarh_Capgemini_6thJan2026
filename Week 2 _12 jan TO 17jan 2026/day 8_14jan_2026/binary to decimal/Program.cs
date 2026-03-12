namespace binary_to_decimal
{
    using System;

    class LogicBase
    {
        public virtual int Calculate(int input1)
        {
            return 0;
        }
    }
    class BinaryToDecimal : LogicBase
    {
        public override int Calculate(int input1)
        {
            string bin = input1.ToString();

            if (bin.Length > 5)
                return -2;

            foreach (char c in bin)
            {
                if (c != '0' && c != '1')
                    return -1;
            }

            return Convert.ToInt32(bin, 2);
        }
    }
    class Program
    {
        static void Main()
        {
            int input = Convert.ToInt32(Console.ReadLine());
            LogicBase obj = new BinaryToDecimal();
            Console.WriteLine(obj.Calculate(input));
        }
    }

}

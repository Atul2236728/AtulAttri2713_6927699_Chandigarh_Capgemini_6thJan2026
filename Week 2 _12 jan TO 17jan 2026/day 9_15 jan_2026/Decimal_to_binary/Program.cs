namespace Decimal_to_binary
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Enter the decimal number:");
            int input1 = int.Parse(Console.ReadLine());

            if (input1 < 0)
            {
                Console.WriteLine("-1");
                return;
            }

            if (input1 == 0)
            {
                Console.WriteLine("0");
                return;
            }

            List<int> output = new List<int>();

            while (input1 > 0)
            {
                output.Add(input1 % 2);
                input1 /= 2;
            }

            output.Reverse();

            Console.WriteLine("Output:");
            foreach (int bit in output)
                Console.Write(bit + " ");
        }
    }
}

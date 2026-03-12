namespace Operation_Selector
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Enter input1:");
            int input1 = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter input2:");
            int input2 = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter input3 (1-Add, 2-Subtract, 3-Multiply, 4-Divide):");
            int input3 = int.Parse(Console.ReadLine());

            if (input1 < 0 && input2 < 0)
            {
                Console.WriteLine("-1");
                return;
            }

            int output = 0;

            switch (input3)
            {
                case 1:
                    output = input1 + input2;
                    break;
                case 2:
                    output = input1 - input2;
                    break;
                case 3:
                    output = input1 * input2;
                    break;
                case 4:
                    if (input2 != 0)
                        output = input1 / input2;
                    else
                        output = 0;
                    break;
            }

            Console.WriteLine("Output:");
            Console.WriteLine(output);
        }
    }
}

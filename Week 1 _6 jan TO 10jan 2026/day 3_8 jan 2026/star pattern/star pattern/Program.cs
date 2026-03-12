namespace star_pattern
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("enter the no of rows ");
            int row = int.Parse(Console.ReadLine());
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < row - i; j++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }
            //Console.ReadLine();
            Console.WriteLine("this is the next problem");

        }
    }
}

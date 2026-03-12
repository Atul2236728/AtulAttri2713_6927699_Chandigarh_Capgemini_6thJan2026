namespace biggest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int x, y, z;
            Console.WriteLine("this is the first problem");
            Console.WriteLine("enter the value of the x ");
            x = int.Parse(Console.ReadLine());
            Console.WriteLine("enter the value of y");
            y = int.Parse(Console.ReadLine());
            Console.Write("enter the value of z");
            z = int.Parse(Console.ReadLine());
            if ((x > y) && (x > z))
            {
                Console.WriteLine("x is the greatest");
            }
            else if (y > z)
            {
                Console.WriteLine("y is greatest");
            }
            else
            {
                Console.WriteLine("z is greatest");

            }
        }
    }
}

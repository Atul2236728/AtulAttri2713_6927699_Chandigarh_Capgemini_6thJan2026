namespace equation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the value of a, b, and c");
            int a=int.Parse(Console.ReadLine());
            int b=int.Parse(Console.ReadLine());
            int c=int.Parse(Console.ReadLine());
            int ans = (a * 3) + (a * 2 * b) + (2 * a * 2 * b) + (2 * a * b * 2) + (a * b * 2) + (b * 3);
            Console.WriteLine(ans);
        }
    }
}

namespace calculate
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("enter the number ");
            int amount = Convert.ToInt32(Console.ReadLine());
            int output = CountNotes(amount);
            Console.WriteLine("Output: " + output);
        }

        static int CountNotes(int amt)
        {
            if (amt < 0)
                return -1;

            int count = 0;

            count += amt / 500;
            amt %= 500;

            count += amt / 100;
            amt %= 100;

            count += amt / 50;
            amt %= 50;

            count += amt / 10;
            amt %= 10;

            count += amt / 1;

            return count;
        }
    }
}

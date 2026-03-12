namespace mcq
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nWhat is the correct way to declare a variale to stroe an integer valuein C#?\na. int 1x = 10;\nb. int x = 10;\nc. float x = 10.0f;\nd. string x = 10; ");
            Console.Write("Choose the answr letter :: ");
            char ch;
            ch = Convert.ToChar(Console.ReadLine());

            if (ch == 'b')
            {
                Console.WriteLine("Correct Option :)");
            }
            else
            {
                Console.WriteLine("Incorrect choice!!");
            }
            Console.ReadLine();


        }
    }
}

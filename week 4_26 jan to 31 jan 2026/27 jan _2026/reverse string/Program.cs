namespace reverse_string
{
    class Program
    {
        static void Main()
        {
            string input = "hello";
            string reversed = "";

            for (int i = input.Length - 1; i >= 0; i--)
            {
                reversed += input[i];
            }

            Console.WriteLine(reversed);
        }
    }
}

namespace count_vowels
{
    class Program
    {
        static void Main()
        {
            Console.Write("Enter a string: ");
            string input = Console.ReadLine();

            int count = 0;

            for (int i = 0; i < input.Length; i++)
            {
                char ch = char.ToLower(input[i]);

                if (ch == 'a' || ch == 'e' || ch == 'i' ||
                    ch == 'o' || ch == 'u')
                {
                    count++;
                }
            }

            Console.WriteLine("Number of vowels: " + count);
        }
    }
}

namespace fiirst_non_repeating_ele
{
    class Program
    {
        static void Main()
        {
            Console.Write("Enter a string: ");
            string input = Console.ReadLine();

            Dictionary<char, int> freq = new Dictionary<char, int>();

            foreach (char c in input)
            {
                if (freq.ContainsKey(c))
                    freq[c]++;
                else
                    freq[c] = 1;
            }

            foreach (char c in input)
            {
                if (freq[c] == 1)
                {
                    Console.WriteLine("First non-repeating character: " + c);
                    return;
                }
            }

            Console.WriteLine("No non-repeating character found");
        }
    }
}

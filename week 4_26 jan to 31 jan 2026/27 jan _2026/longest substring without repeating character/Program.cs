namespace longest_substring_without_repeating_character
{
    class Program
    {
        static void Main()
        {
            string s = Console.ReadLine();

            HashSet<char> set = new HashSet<char>();
            int left = 0, maxLength = 0, startIndex = 0;

            for (int right = 0; right < s.Length; right++)
            {
                while (set.Contains(s[right]))
                {
                    set.Remove(s[left]);
                    left++;
                }

                set.Add(s[right]);

                if (right - left + 1 > maxLength)
                {
                    maxLength = right - left + 1;
                    startIndex = left;
                }
            }

            string longestSubstring = s.Substring(startIndex, maxLength);

            Console.WriteLine("Length: " + maxLength);
            Console.WriteLine("Substring: " + longestSubstring);
        }
    }
}

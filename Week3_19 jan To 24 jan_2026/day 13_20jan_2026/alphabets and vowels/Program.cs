namespace alphabets_and_vowels
{
    using System;

    class Program
    {
        static bool IsVowel(char c)
        {
            c = char.ToLower(c);
            return c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u';
        }

        static void Main()
        {
            string first = Console.ReadLine();
            string second = Console.ReadLine();

            string temp = "";

            // STEP 1: Remove common consonants
            for (int i = 0; i < first.Length; i++)
            {
                char ch = first[i];
                char lowerCh = char.ToLower(ch);

                bool isConsonant = !IsVowel(lowerCh);
                bool foundInSecond = false;

                for (int j = 0; j < second.Length; j++)
                {
                    if (lowerCh == char.ToLower(second[j]))
                    {
                        foundInSecond = true;
                        break;
                    }
                }

                if (!(isConsonant && foundInSecond))
                {
                    temp += ch;
                }
            }

            // STEP 2: Remove consecutive duplicates
            string result = "";

            for (int i = 0; i < temp.Length; i++)
            {
                if (i == 0 || temp[i] != temp[i - 1])
                {
                    result += temp[i];
                }
            }

            Console.WriteLine(result);
        }
    }

}

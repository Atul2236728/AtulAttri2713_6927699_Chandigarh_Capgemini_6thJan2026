namespace lucky_string
{
    using System;

    class Program
    {
        static void Main()
        {
            int n = int.Parse(Console.ReadLine());
            string str = Console.ReadLine();

            // Step 1: Invalid case
            if (n > str.Length)
            {
                Console.WriteLine("Invalid");
                return;
            }

            int half = n / 2;
            bool isLucky = false;

            // Step 2: Check each substring of length n
            for (int i = 0; i <= str.Length - n; i++)
            {
                string sub = str.Substring(i, n);

                // Check if substring contains only P, S, G
                bool validChars = true;
                for (int j = 0; j < sub.Length; j++)
                {
                    char ch = sub[j];
                    if (ch != 'P' && ch != 'S' && ch != 'G')
                    {
                        validChars = false;
                        break;
                    }
                }

                if (!validChars)
                    continue;

                // Check for n/2 consecutive same characters
                int count = 1;
                for (int j = 1; j < sub.Length; j++)
                {
                    if (sub[j] == sub[j - 1])
                    {
                        count++;
                        if (count >= half)
                        {
                            isLucky = true;
                            break;
                        }
                    }
                    else
                    {
                        count = 1;
                    }
                }

                if (isLucky)
                    break;
            }

            // Step 3: Output result
            if (isLucky)
                Console.WriteLine("Yes");
            else
                Console.WriteLine("No");
        }
    }

}

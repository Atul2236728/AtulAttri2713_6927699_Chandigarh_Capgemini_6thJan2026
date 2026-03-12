using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter string: ");
        string s = Console.ReadLine();

        Console.WriteLine("Length: " + LongestPalindrome(s));
    }

    static int LongestPalindrome(string s)
    {
        int maxLen = 1;

        for (int i = 0; i < s.Length; i++)
        {
            Expand(s, i, i, ref maxLen);
            Expand(s, i, i + 1, ref maxLen);
        }

        return maxLen;
    }

    static void Expand(string s, int left, int right, ref int maxLen)
    {
        while (left >= 0 && right < s.Length && s[left] == s[right])
        {
            maxLen = Math.Max(maxLen, right - left + 1);
            left--;
            right++;
        }
    }
}

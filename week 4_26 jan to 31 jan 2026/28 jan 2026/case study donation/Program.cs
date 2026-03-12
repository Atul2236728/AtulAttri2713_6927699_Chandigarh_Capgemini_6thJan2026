//namespace case_study_donation
//{
//    internal class Program
//    {
//        static void Main(string[] args)
//        {
//            Console.WriteLine("Hello, World!");
//        }
//    }
//}
using System;
using System.Collections.Generic;

class UserProgramCode
{
    public static int getDonation(string[] input1, int input2)
    {
        HashSet<string> set = new HashSet<string>();
        int total = 0;

        foreach (string s in input1)
        {
            // Rule 2: check special characters
            foreach (char c in s)
            {
                if (!char.IsDigit(c))
                    return -2;
            }

            // Rule 1: check duplicates
            if (!set.Add(s))
                return -1;

            // Extract parts
            int locationCode = int.Parse(s.Substring(3, 3));
            int donation = int.Parse(s.Substring(6, 3));

            if (locationCode == input2)
                total += donation;
        }

        return total;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("enter the no. of strings");
        int n = int.Parse(Console.ReadLine());
        string[] input1 = new string[n];

        for (int i = 0; i < n; i++)
        {
            input1[i] = Console.ReadLine();
        }
        Console.WriteLine("enter the location code ");
        int input2 = int.Parse(Console.ReadLine());

        int result = UserProgramCode.getDonation(input1, input2);
        Console.WriteLine(result);
    }
}

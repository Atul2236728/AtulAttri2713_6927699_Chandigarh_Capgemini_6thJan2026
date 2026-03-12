using System;
using System.Collections.Generic;

class ProgramCode
{
    public static int convertRomanToDecimal(string roman)
    {
        Dictionary<char, int> values = new Dictionary<char, int>()
        {
            {'I',1},{'V',5},{'X',10},
            {'L',50},{'C',100},
            {'D',500},{'M',1000}
        };

        int sum = 0;

        foreach (char c in roman)
        {
            if (!values.ContainsKey(c))
                return -1;

            sum += values[c];
        }

        return sum;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("enter input"); 
        string input = Console.ReadLine();
        Console.WriteLine(ProgramCode.convertRomanToDecimal(input));
    }
}

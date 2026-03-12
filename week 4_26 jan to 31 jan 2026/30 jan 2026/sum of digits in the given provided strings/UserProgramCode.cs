using System;

public class UserProgramCode
{
    public static int sumOfDigits(string[] input1)
    {
        int sum = 0;

        foreach (string str in input1)
        {
            foreach (char ch in str)
            {
                if (char.IsDigit(ch))
                {
                    sum += ch - '0'; // convert char digit to int
                }
                else if (char.IsLetter(ch))
                {
                    continue; // ignore alphabets
                }
                else
                {
                    // special character found
                    return -1;
                }
            }
        }
        return sum;
    }
}

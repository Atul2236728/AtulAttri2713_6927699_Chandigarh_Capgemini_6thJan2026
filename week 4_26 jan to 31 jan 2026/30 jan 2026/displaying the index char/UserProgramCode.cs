using System;
using System.Text;

public class UserProgramCode
{
    public static string formString(string[] input1, int input2)
    {
        StringBuilder result = new StringBuilder();

        foreach (string str in input1)
        {
            // Business rule: no special characters allowed
            foreach (char c in str)
            {
                if (!char.IsLetter(c))
                    return "-1";
            }

            // Pick nth character or $
            if (str.Length >= input2)
                result.Append(str[input2 - 1]);
            else
                result.Append('$');
        }

        return result.ToString();
    }
}

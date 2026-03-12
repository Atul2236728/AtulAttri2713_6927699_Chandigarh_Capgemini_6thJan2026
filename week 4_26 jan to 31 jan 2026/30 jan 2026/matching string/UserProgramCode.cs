using System;
using System.Collections.Generic;

public class UserProgramCode
{
    public static string[] getEmployee(string[] input1, string input2)
    {
        // Check for special characters in input2
        foreach (char c in input2)
        {
            if (!char.IsLetter(c) && c != ' ')
                return new string[] { "Invalid Input" };
        }

        // Check for special characters in input1
        foreach (string s in input1)
        {
            foreach (char c in s)
            {
                if (!char.IsLetter(c) && c != ' ')
                    return new string[] { "Invalid Input" };
            }
        }

        List<string> matchedEmployees = new List<string>();
        HashSet<string> allDesignations = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        for (int i = 0; i < input1.Length; i += 2)
        {
            string employee = input1[i];
            string designation = input1[i + 1];

            allDesignations.Add(designation);

            if (designation.Equals(input2, StringComparison.OrdinalIgnoreCase))
            {
                matchedEmployees.Add(employee);
            }
        }

        if (allDesignations.Count == 1)
        {
            return new string[] { "All employees belong to same " + input2 + " designation" };
        }

        if (matchedEmployees.Count == 0)
        {
            return new string[] { "No employee for " + input2 + " designation" };
        }

        return matchedEmployees.ToArray();
    }
}

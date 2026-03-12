using System;
using System.Collections.Generic;
using System.Text;

namespace casestudy_handson_3
{
    internal class Class1
    {
        
        
            public static string nextString(string input1)
            {
                StringBuilder result = new StringBuilder();

                foreach (char ch in input1)
                {
                    
                    if (!char.IsLetter(ch))
                        return "Invalid input";

                    if (IsVowel(ch))
                        result.Append(GetNextConsonant(ch));
                    else
                        result.Append(GetNextVowel(ch));
                }

                return result.ToString();
            }

            static bool IsVowel(char c)
            {
                return "aeiouAEIOU".IndexOf(c) != -1;
            }

            static char GetNextConsonant(char c)
            {
                char next = c;

                do
                {
                    next++;
                    if (next > 'z' && char.IsLower(c))
                        next = 'a';
                    else if (next > 'Z' && char.IsUpper(c))
                        next = 'A';
                }
                while (IsVowel(next));

                return next;
            }

            static char GetNextVowel(char c)
            {
                char next = c;

                while (true)
                {
                    next++;
                    if (next > 'z' && char.IsLower(c))
                        next = 'a';
                    else if (next > 'Z' && char.IsUpper(c))
                        next = 'A';

                    if (IsVowel(next))
                        return next;
                }
            }
        }
    }


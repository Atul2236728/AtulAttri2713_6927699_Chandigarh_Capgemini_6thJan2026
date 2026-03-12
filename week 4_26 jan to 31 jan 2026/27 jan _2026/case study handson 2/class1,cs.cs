using System;
using System.Collections.Generic;
using System.Text;

namespace case_study_handson_2
{
    internal class class1_cs
    {
        
            public static int largestNumber(int[] input1)
            {
                bool hasNegative = false;
                bool hasInvalid = false;

                HashSet<int> unique = new HashSet<int>();

                foreach (int num in input1)
                {
                    if (num < 0)
                        hasNegative = true;
                    else if (num == 0 || num > 100)
                        hasInvalid = true;
                    else
                        unique.Add(num); // remove duplicates
                }

                if (hasNegative && hasInvalid)
                    return -3;
                if (hasNegative)
                    return -1;
                if (hasInvalid)
                    return -2;

                int sum = 0;

                for (int start = 1; start <= 91; start += 10)
                {
                    int end = start + 9;
                    int max = -1;

                    foreach (int num in unique)
                    {
                        if (num >= start && num <= end)
                        {
                            if (num > max)
                                max = num;
                        }
                    }

                    if (max != -1)
                        sum += max;
                }

                return sum;
            }
        }
    }


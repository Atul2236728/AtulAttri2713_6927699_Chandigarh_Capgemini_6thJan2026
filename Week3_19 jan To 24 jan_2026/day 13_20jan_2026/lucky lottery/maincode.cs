using System;
using System.Collections.Generic;
using System.Text;

namespace lucky_lottery
{
    class Maincode
    {
        public static int luckynumbers(int m, int n)
        {
            int output = 0;
            int cnt = 0;
            for (int i = m; i <= n; i++)
            {
                for (int j = 2; j <= i; j++)
                {
                    if (i % j == 0)
                    {
                        cnt++;
                    }
                }
                if (cnt != 2)
                {
                    if (sumof(i * i) == (sumof(i) * sumof(i)))
                    {
                        output++;
                    }
                }
            }
            return output;
        }
        public static int sumof(int number)
        {
            int number1 = number;
            int sum = 0;
            int r = 0;
            while (number1 > 0)
            {
                r = number1 % 10;
                sum = sum + r;
                number1 = number1 / 10;
            }
            return sum;
        }
    }

}


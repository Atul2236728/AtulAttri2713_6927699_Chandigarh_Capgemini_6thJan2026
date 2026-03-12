using delagate;
using System;
using System.Security.Claims;

namespace delagate
{
  
        
        public delegate void Math(int x, int y);

        class MultiClass
        {
            public void Add(int x, int y)
            {
                Console.WriteLine("Add: " + (x + y));
            }

            public void Sub(int x, int y)
            {
                Console.WriteLine("Sub: " + (x - y));
            }

            public void Mul(int x, int y)
            {
                Console.WriteLine("Mul: " + (x * y));
            }

            public void Div(int x, int y)
            {
                Console.WriteLine("Div: " + (x / y));
            }
        }

        class Program
        {
            static void Main()
            {
                MultiClass obj = new MultiClass();

                Math m = new Math(obj.Add);
                m += obj.Sub;
                m += obj.Mul;
                m += obj.Div;

                m(100, 50);
                Console.WriteLine();
               m -= obj.Add;
                m(450, 70);
                Console.WriteLine();

                m -= obj.Div;
                m(625, 25);

                Console.ReadLine();
            }
        }
    }



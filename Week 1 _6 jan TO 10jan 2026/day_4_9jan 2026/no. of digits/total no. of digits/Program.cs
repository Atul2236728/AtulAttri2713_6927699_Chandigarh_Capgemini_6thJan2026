namespace total_no._of_digits
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int num, output1;
            int count = 0;
            Console.WriteLine("enter no.");
            num = Convert.ToInt32(Console.ReadLine());
            if (num < 0)
            {
                output1 = -1;
                Console.WriteLine("output is " + output1);

            }
            else
            {
                
               

                while (num > 0)
                {
                    num = num / 10;
                    count++;
                    }
                }
                Console.WriteLine("the no. of digits :-"+ count);
            }
        }
    }


namespace ConsoleApp1
{
    
           class Program
        {
            static void Main()
            {
            Console.WriteLine("enter the size ");
                int Input2 = int.Parse(Console.ReadLine());

                if (Input2 < 0)
                {
                    Console.WriteLine("-2");
                    return;
                }

                int[] Input1 = new int[Input2];
                int neg = 0;
            Console.WriteLine("enter the elements");
                for (int i = 0; i < Input2; i++)
                {
                    Input1[i] = int.Parse(Console.ReadLine());
                    if (Input1[i] < 0) neg = 1;
                }

                if (neg == 1) Console.Write("-1,");

                for (int i = 0; i < Input2; i++)
                {
                    if (Input1[i] < 0) continue;

                    int duplicate = 0;
                    for (int j = 0; j < i; j++)
                        if (Input1[i] == Input1[j]) duplicate = 1;

                    if (duplicate == 0) Console.Write(Input1[i] + ",");
                }
            }
        }
    }
    

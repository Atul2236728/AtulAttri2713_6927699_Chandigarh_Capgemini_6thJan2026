using System;

namespace comapring_arrays
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //            int[] arr1 = { 1, 2, 3, 4, 5, 6, 7 };
            //            int[] arr2 = { 11, 2, 13, 4, 5, 6, 89 };
            //            int n = arr1.Length;
            //            int j = arr2.Length;
            //            int[] output = new int[n];
            //            if (n < 0 || j < 0)
            //            {
            //                output[0] = -2;
            //            }
            //            else
            //            {
            //                for (int i = 0; i < n; i++)
            //                {
            //                    if (arr1[i] < 0 || arr2[i] < 0)
            //                    {
            //                        output[0] = -1;
            //                        Console.WriteLine(output[0]);
            //                        return;
            //                    }

            //                }
            //                for (int i = 0; i < n; i++)
            //                {
            //                    if (arr1[i] > arr2[i])
            //                    {
            //                        output[i] = arr1[i];
            //                    }
            //                    else
            //                    {
            //                        output[i] = arr2[i];
            //                    }
            //                }
            //            }
            //            for (int i = 0; i < n; i++)
            //            {
            //                Console.WriteLine("the greatest element is " + output[i]);
            //            }

            //        }
            //    }
            //}
            int[] arr1 = { 1, 5, 3, 7 };
            int[] arr2 = { 2, 4, 6, 1 };

            int n = arr1.Length;
            int[] output = new int[n];

            if (n < 0)
            {
                output[0] = -2;
            }
            else
            {
                
                for (int i = 0; i < n; i++)
                {
                    if (arr1[i] < 0 || arr2[i] < 0)
                    {
                        output[0] = -1;
                        Console.WriteLine(output[0]);
                        return;
                    }
                }

                for (int i = 0; i < n; i++)
                {
                    if (arr1[i] > arr2[i])
                        output[i] = arr1[i];
                    else
                        output[i] = arr2[i];
                }
            }

            Console.WriteLine("Output Array:");
            for (int i = 0; i < n; i++)
            {
                Console.Write(output[i] + " ");
            }
        }
    }
}

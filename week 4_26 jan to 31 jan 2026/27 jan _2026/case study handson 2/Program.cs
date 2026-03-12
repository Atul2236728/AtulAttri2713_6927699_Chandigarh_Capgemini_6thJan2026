namespace case_study_handson_2
{
    class program
    {
        static void Main()
        {
            Console.WriteLine("enter the size ");
            int n = Convert.ToInt32(Console.ReadLine());
            int[] arr = new int[n];

            for (int i = 0; i < n; i++)
                arr[i] = Convert.ToInt32(Console.ReadLine());

            int result = class1_cs.largestNumber(arr);
            Console.WriteLine(result);
        }
    }
}
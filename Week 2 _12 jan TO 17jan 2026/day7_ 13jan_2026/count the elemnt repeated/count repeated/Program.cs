namespace count_repeated
{
    internal class Program
    {
       static public int Countelement(int[] arr, int n, int searchvalue)
        {
            if (n < 0)
                return -2;
            if (searchvalue < 0)
                return -1;
            for (int i = 0; i < n; i++)
            {
                if (arr[i] < 0)
                    return -1;

            }
            int count = 0;
            for (int i = 0; i < n; i++)
            {
                if (arr[i] == searchvalue)
                    count++;
            }
            return count;
        }
        static void Main(string[] args)
        {
           int[] input1 ={ 1,2,2,3,3};
            int input2 = 5;
            int input3 = 2;
            int output = Countelement(input1, input2, input3);
            Console.WriteLine(output);
        }
        
    }
}

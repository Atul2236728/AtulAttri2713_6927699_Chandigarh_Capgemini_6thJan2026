namespace transverse__A_Matrix
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the size");
            int s=int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the array");
            int[] arr1=new int[s];
            int count = 1;
            for(int i=0; i<s;i++)
            {
                arr1[i]=int.Parse(Console.ReadLine());
            }
            for(int i=0; i<s; i++)
            {
                if (arr1[i] == -1)
                    continue;
                for(int j=i+1; j<s; j++)
                {
                    if (arr1[i] == arr1[j])
                    {
                        count++;
                        arr1[j] = -1;
                    }
                }
                Console.WriteLine(arr1[i] + " occurs " + count + " times");
                count = 1;
               
            }
        }
    }
}

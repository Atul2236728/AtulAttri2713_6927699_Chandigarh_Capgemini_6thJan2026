namespace comma_separated
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string st=Console.ReadLine();
            string result = "";
            int count = 1;
            for(int i=0;i<st.Length-1;i++)
            {
                if (st[i] == st[i+1])
                {
                    count++;
;                }
                else
                {
                    result += st[i] + count.ToString();
                    count = 1;
                }
            }
           result += st[st.Length-1]+count.ToString();
            Console.WriteLine(result);
        }
    }
}

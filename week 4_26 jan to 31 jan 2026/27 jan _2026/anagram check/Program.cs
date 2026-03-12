namespace anagram_check
{
    class Program
    {
        static void Main()
        {
            Console.Write("Enter first string: ");
            string str1 = Console.ReadLine().Trim().ToLower();

            Console.Write("Enter second string: ");
            string str2 = Console.ReadLine().Trim().ToLower();

            if (str1.Length != str2.Length)
            {
                Console.WriteLine("Not Anagrams");
                return;
            }

            char[] arr1 = str1.ToCharArray();
            char[] arr2 = str2.ToCharArray();

            Array.Sort(arr1);
            Array.Sort(arr2);

            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i])
                {
                    Console.WriteLine("Not Anagrams");
                    return;
                }
            }

            Console.WriteLine("Anagrams");
        }
    }
}

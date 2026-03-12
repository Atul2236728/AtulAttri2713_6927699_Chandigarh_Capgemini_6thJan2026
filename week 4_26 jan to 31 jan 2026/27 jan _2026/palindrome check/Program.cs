namespace palindrome_check
{
    class Program
    {
        static void Main()
        {
            Console.Write("Enter a string: ");
            string input = Console.ReadLine();

            bool isPalindrome = true;
            int left = 0;
            int right = input.Length - 1;

            while (left < right)
            {
                if (char.ToLower(input[left]) != char.ToLower(input[right]))
                {
                    isPalindrome = false;
                    break;
                }
                left++;
                right--;
            }

            if (isPalindrome)
                Console.WriteLine("Palindrome");
            else
                Console.WriteLine("Not a Palindrome");
        }
    }
}

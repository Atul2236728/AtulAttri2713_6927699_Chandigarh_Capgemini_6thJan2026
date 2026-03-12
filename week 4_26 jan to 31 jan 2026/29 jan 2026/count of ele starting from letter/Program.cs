using System;

class UserProgramCode
{
    public static int GetCount(int size, string[] arr, char ch)
    {
        int count = 0;
        ch = char.ToLower(ch);

        foreach (string s in arr)
        {
            foreach (char c in s)
            {
                if (!char.IsLetter(c))
                    return -2;
            }

            if (char.ToLower(s[0]) == ch)
                count++;
        }

        if (count == 0)
            return -1;

        return count;
    }
}

class Program
{
    static void Main()
    {
        int size = int.Parse(Console.ReadLine());
        string[] arr = new string[size];

        for (int i = 0; i < size; i++)
        {
            arr[i] = Console.ReadLine();
        }

        char ch = char.Parse(Console.ReadLine());

        int result = UserProgramCode.GetCount(size, arr, ch);

        if (result == -1)
            Console.WriteLine("No elements Found");
        else if (result == -2)
            Console.WriteLine("Only alphabets should be given");
        else
            Console.WriteLine(result);
    }
}

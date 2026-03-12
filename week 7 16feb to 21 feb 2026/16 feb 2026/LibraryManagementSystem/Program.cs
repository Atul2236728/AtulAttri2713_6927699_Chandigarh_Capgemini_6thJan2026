namespace LibraryManagementSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            
                var library = new Library();

                library.AddBook("C# Basics", 3);

                library.BorrowBook("C# Basics");

                Console.WriteLine($"Remaining copies: {library.GetBookCount("C# Basics")}");

                try
                {
                    library.BorrowBook("Java");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }


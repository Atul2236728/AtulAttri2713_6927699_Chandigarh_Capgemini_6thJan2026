namespace bank_management_sys
{
    class Program
    {
        static void Main()
        {
            BankAccount acc = new BankAccount("Atul", 101, 5000);

            int choice;
            do
            {
                Console.WriteLine("\n--- BANK MENU ---");
                Console.WriteLine("1. Deposit");
                Console.WriteLine("2. Withdraw");
                Console.WriteLine("3. Display");
                Console.WriteLine("4. Exit");
                Console.Write("Enter choice: ");
                choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter amount: ");
                        double d = Convert.ToDouble(Console.ReadLine());
                        acc.Deposit(d);
                        break;

                    case 2:
                        Console.Write("Enter amount: ");
                        double w = Convert.ToDouble(Console.ReadLine());
                        acc.Withdraw(w);
                        break;

                    case 3:
                        acc.Display();
                        break;

                    case 4:
                        Console.WriteLine("Thank You!");
                        break;

                    default:
                        Console.WriteLine("Invalid Choice.");
                        break;
                }
                break;

            } while (choice != 4);
        }
    }
}

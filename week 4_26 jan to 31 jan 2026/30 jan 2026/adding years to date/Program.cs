using System;
using System.Globalization;

class Program
{
    static void Main()
    {
        Console.Write("Enter date (dd/MM/yyyy): ");
        string dateInput = Console.ReadLine();

        Console.Write("Enter number of years to add: ");
        int years = int.Parse(Console.ReadLine());

        if (years < 0)
        {
            Console.WriteLine(-2);
            return;
        }

        if (!DateTime.TryParseExact(
            dateInput,
            "dd/MM/yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out DateTime date))
        {
            Console.WriteLine(-1);
            return;
        }

        DateTime newDate = date.AddYears(years);
        Console.WriteLine("Day: " + newDate.DayOfWeek);
    }
}

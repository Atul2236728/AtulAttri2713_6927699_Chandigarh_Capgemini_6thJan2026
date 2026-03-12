using System;
using System.Globalization;

class Program
{
    static void Main()
    {
        Console.Write("Enter date (dd/MM/yyyy): ");
        string inputDate = Console.ReadLine();

        if (!DateTime.TryParseExact(
            inputDate,
            "dd/MM/yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out DateTime date))
        {
            Console.WriteLine(-1);
            return;
        }

        DateTime newDate = date.AddYears(1);

        Console.WriteLine("Day: " + newDate.DayOfWeek);
    }
}

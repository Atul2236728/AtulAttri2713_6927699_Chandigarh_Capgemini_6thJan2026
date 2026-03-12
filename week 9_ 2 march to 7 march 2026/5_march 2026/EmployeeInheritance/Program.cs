using System;

abstract class Employee
{
    protected string department;
    protected string name;
    protected string location;
    protected bool isOnVacation = false;

    public Employee(string dept, string name, string loc)
    {
        department = dept;
        this.name = name;
        location = loc;
    }

    public string GetDepartment()
    {
        return department;
    }

    public string GetName()
    {
        return name;
    }

    public string GetLocation()
    {
        return location;
    }

    public bool GetStatus()
    {
        return isOnVacation;
    }

    public void SwitchStatus()
    {
        isOnVacation = !isOnVacation;
    }
}

class FinanceEmployee : Employee
{
    public FinanceEmployee(string dept, string name, string loc)
        : base(dept, name, loc) { }
}

class MarketingEmployee : Employee
{
    public MarketingEmployee(string dept, string name, string loc)
        : base(dept, name, loc) { }
}

class Solution
{
    static void Main()
    {
        Console.WriteLine("Enter Finance Employee details:");
        string dept = Console.ReadLine();
        string name = Console.ReadLine();
        string loc = Console.ReadLine();

        Employee finance = new FinanceEmployee(dept, name, loc);

        Console.WriteLine($"FinanceEmployee info: Department-{finance.GetDepartment()}, Name-{finance.GetName()}, Location-{finance.GetLocation()}");

        Console.WriteLine($"{finance.GetName()} vacation status: {finance.GetStatus()}");

        finance.SwitchStatus();

        Console.WriteLine($"After switching vacation: {finance.GetStatus()}");


        Console.WriteLine("\nEnter Marketing Employee details:");
        dept = Console.ReadLine();
        name = Console.ReadLine();
        loc = Console.ReadLine();

        Employee marketing = new MarketingEmployee(dept, name, loc);

        Console.WriteLine($"MarketingEmployee info: Department-{marketing.GetDepartment()}, Name-{marketing.GetName()}, Location-{marketing.GetLocation()}");

        Console.WriteLine($"{marketing.GetName()} vacation status: {marketing.GetStatus()}");

        marketing.SwitchStatus();

        Console.WriteLine($"After switching vacation: {marketing.GetStatus()}");
    }
}
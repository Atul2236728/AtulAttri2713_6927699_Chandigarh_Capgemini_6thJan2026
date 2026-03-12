using System;

class User
{
    public int Id;
    public string Name;
    public string Email;

    public void ShowUser()
    {
        Console.WriteLine($"Id:{Id} Name:{Name} Email:{Email}");
    }
}

class Admin : User
{
    public string Role;

    public void ShowAdmin()
    {
        ShowUser();
        Console.WriteLine($"Role:{Role}");
    }
}

class Customer : User
{
    public string Address;

    public void ShowCustomer()
    {
        ShowUser();
        Console.WriteLine($"Address:{Address}");
    }
}

class Solution
{
    static void Main()
    {
        Admin admin = new Admin();

        Console.WriteLine("Enter Admin Details");

        admin.Id = int.Parse(Console.ReadLine());
        admin.Name = Console.ReadLine();
        admin.Email = Console.ReadLine();
        admin.Role = Console.ReadLine();

        Console.WriteLine("\nAdmin Information:");
        admin.ShowAdmin();

        Customer customer = new Customer();

        Console.WriteLine("\nEnter Customer Details");

        customer.Id = int.Parse(Console.ReadLine());
        customer.Name = Console.ReadLine();
        customer.Email = Console.ReadLine();
        customer.Address = Console.ReadLine();

        Console.WriteLine("\nCustomer Information:");
        customer.ShowCustomer();
    }
}
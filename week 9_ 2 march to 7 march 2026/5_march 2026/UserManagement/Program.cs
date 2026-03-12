using System;
using System.Collections.Generic;
using System.Linq;

class User
{
    public int Id;
    public string Name;
    public string Email;
}

class UserManager
{
    public static (List<User>, List<User>) CompareUsers(List<User> dbUsers, List<User> newUsers)
    {
        List<User> updated = new List<User>();
        List<User> inserted = new List<User>();

        foreach (var user in newUsers)
        {
            var existing = dbUsers.FirstOrDefault(x => x.Id == user.Id);

            if (existing != null)
            {
                if (existing.Name != user.Name || existing.Email != user.Email)
                {
                    updated.Add(user);
                }
            }
            else
            {
                inserted.Add(user);
            }
        }

        return (updated, inserted);
    }
}

class Solution
{
    static void Main()
    {
        List<User> dbUsers = new List<User>();
        List<User> newUsers = new List<User>();

        Console.Write("Enter number of users in DB: ");
        int n = int.Parse(Console.ReadLine());

        for (int i = 0; i < n; i++)
        {
            User u = new User();

            Console.Write("Id: ");
            u.Id = int.Parse(Console.ReadLine());

            Console.Write("Name: ");
            u.Name = Console.ReadLine();

            Console.Write("Email: ");
            u.Email = Console.ReadLine();

            dbUsers.Add(u);
        }

        Console.Write("\nEnter number of new users: ");
        int m = int.Parse(Console.ReadLine());

        for (int i = 0; i < m; i++)
        {
            User u = new User();

            Console.Write("Id: ");
            u.Id = int.Parse(Console.ReadLine());

            Console.Write("Name: ");
            u.Name = Console.ReadLine();

            Console.Write("Email: ");
            u.Email = Console.ReadLine();

            newUsers.Add(u);
        }

        var result = UserManager.CompareUsers(dbUsers, newUsers);

        Console.WriteLine($"\nUpdated Users: {result.Item1.Count}");
        Console.WriteLine($"Inserted Users: {result.Item2.Count}");
    }
}
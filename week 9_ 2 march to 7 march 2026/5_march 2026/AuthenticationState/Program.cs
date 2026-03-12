using System;
using System.Collections.Generic;

class User
{
    public int Id;
    public string Email;
    public string Password;
    public string Location;
    public bool LoggedIn = false;
}

class AuthSystem
{
    List<User> users = new List<User>();
    List<string> allowedLocations = new List<string>();

    public AuthSystem(List<string> locations)
    {
        allowedLocations = locations;
    }

    public string Register(User user)
    {
        foreach (var u in users)
        {
            if (u.Email == user.Email)
                return $"{user.Email} is already registered!";
        }

        users.Add(user);
        return $"{user.Email} registered successfully!";
    }

    public string Login(User user)
    {
        var u = users.Find(x => x.Email == user.Email);

        if (u == null)
            return $"{user.Email} is not registered!";

        if (!allowedLocations.Contains(user.Location))
            return $"{user.Email} is not allowed to login from this location!";

        if (u.Password != user.Password)
            return $"{user.Email} password is incorrect!";

        if (u.LoggedIn)
            return $"{user.Email} is already logged in!";

        u.LoggedIn = true;
        return $"{user.Email} logged in successfully!";
    }

    public string Logout(User user)
    {
        var u = users.Find(x => x.Email == user.Email);

        if (u == null)
            return $"{user.Email} is not registered!";

        if (!u.LoggedIn)
            return $"{user.Email} is not logged in!";

        u.LoggedIn = false;
        return $"{user.Email} logged out successfully!";
    }
}

class Solution
{
    static void Main()
    {
        List<string> locations = new List<string>();

        Console.Write("Enter number of allowed locations: ");
        int n = int.Parse(Console.ReadLine());

        for (int i = 0; i < n; i++)
            locations.Add(Console.ReadLine());

        AuthSystem auth = new AuthSystem(locations);

        User user = new User();

        Console.Write("Email: ");
        user.Email = Console.ReadLine();

        Console.Write("Password: ");
        user.Password = Console.ReadLine();

        Console.Write("Location: ");
        user.Location = Console.ReadLine();

        Console.WriteLine(auth.Register(user));
        Console.WriteLine(auth.Login(user));
        Console.WriteLine(auth.Logout(user));
    }
}
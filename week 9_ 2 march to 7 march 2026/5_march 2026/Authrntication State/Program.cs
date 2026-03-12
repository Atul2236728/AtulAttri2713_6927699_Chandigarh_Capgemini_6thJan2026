using System;
using System.Collections.Generic;

class User
{
    public int Id;
    public string Email;
    public string Password;
    public string Location;
    public bool IsLoggedIn = false;
}

class ApplicationAuthState
{
    List<User> registeredUsers = new List<User>();
    List<string> allowedLocations;

    public ApplicationAuthState(List<string> locations)
    {
        allowedLocations = locations;
    }

    public string Register(User user)
    {
        foreach (var u in registeredUsers)
        {
            if (u.Email == user.Email)
                return $"{user.Email} is already registered!";
        }

        registeredUsers.Add(user);
        return $"{user.Email} registered successfully!";
    }

    public string Login(User user)
    {
        var existing = registeredUsers.Find(x => x.Email == user.Email);

        if (existing == null)
            return $"{user.Email} is not registered!";

        if (!allowedLocations.Contains(user.Location))
            return $"{user.Email} is not allowed to login from this location!";

        if (existing.Password != user.Password)
            return $"{user.Email} password is incorrect!";

        if (existing.IsLoggedIn)
            return $"{user.Email} is already logged in!";

        existing.IsLoggedIn = true;
        return $"{user.Email} logged in successfully!";
    }

    public string Logout(User user)
    {
        var existing = registeredUsers.Find(x => x.Email == user.Email);

        if (existing == null)
            return $"{user.Email} is not registered!";

        if (!existing.IsLoggedIn)
            return $"{user.Email} is not logged in!";

        existing.IsLoggedIn = false;
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

        ApplicationAuthState app = new ApplicationAuthState(locations);

        User user = new User();

        Console.Write("Email: ");
        user.Email = Console.ReadLine();

        Console.Write("Password: ");
        user.Password = Console.ReadLine();

        Console.Write("Location: ");
        user.Location = Console.ReadLine();

        Console.WriteLine(app.Register(user));
        Console.WriteLine(app.Login(user));
        Console.WriteLine(app.Logout(user));
    }
}
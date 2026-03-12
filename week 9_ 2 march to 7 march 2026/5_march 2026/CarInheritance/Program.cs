using System;

class Car
{
    public string Brand;
    public int Price;

    public void ShowCar()
    {
        Console.WriteLine($"Brand:{Brand} Price:{Price}");
    }
}

class ElectricCar : Car
{
    public int Battery;

    public void ShowElectric()
    {
        ShowCar();
        Console.WriteLine($"Battery:{Battery} kWh");
    }
}

class PetrolCar : Car
{
    public int FuelCapacity;

    public void ShowPetrol()
    {
        ShowCar();
        Console.WriteLine($"Fuel Capacity:{FuelCapacity} L");
    }
}

class Solution
{
    static void Main()
    {
        ElectricCar e = new ElectricCar();

        Console.WriteLine("Enter Electric Car Details");

        e.Brand = Console.ReadLine();
        e.Price = int.Parse(Console.ReadLine());
        e.Battery = int.Parse(Console.ReadLine());

        Console.WriteLine("\nElectric Car Info:");
        e.ShowElectric();

        PetrolCar p = new PetrolCar();

        Console.WriteLine("\nEnter Petrol Car Details");

        p.Brand = Console.ReadLine();
        p.Price = int.Parse(Console.ReadLine());
        p.FuelCapacity = int.Parse(Console.ReadLine());

        Console.WriteLine("\nPetrol Car Info:");
        p.ShowPetrol();
    }
}
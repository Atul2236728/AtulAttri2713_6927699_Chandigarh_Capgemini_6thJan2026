using System;

class Computer
{
    public string Brand;
    public string Processor;
    public int RAM;
    public int Storage;

    public void Display()
    {
        Console.WriteLine($"Brand:{Brand} Processor:{Processor} RAM:{RAM}GB Storage:{Storage}GB");
    }
}

class Laptop : Computer
{
    public int Battery;

    public void ShowLaptop()
    {
        Display();
        Console.WriteLine($"Battery:{Battery} hours");
    }
}

class Desktop : Computer
{
    public string CabinetType;

    public void ShowDesktop()
    {
        Display();
        Console.WriteLine($"Cabinet:{CabinetType}");
    }
}

class Server : Computer
{
    public int Nodes;

    public void ShowServer()
    {
        Display();
        Console.WriteLine($"Nodes:{Nodes}");
    }
}

class Solution
{
    static void Main()
    {
        Console.WriteLine("Enter Laptop Details");

        Laptop l = new Laptop();

        Console.Write("Brand: ");
        l.Brand = Console.ReadLine();

        Console.Write("Processor: ");
        l.Processor = Console.ReadLine();

        Console.Write("RAM: ");
        l.RAM = int.Parse(Console.ReadLine());

        Console.Write("Storage: ");
        l.Storage = int.Parse(Console.ReadLine());

        Console.Write("Battery Hours: ");
        l.Battery = int.Parse(Console.ReadLine());

        Console.WriteLine("\nLaptop Details:");
        l.ShowLaptop();
    }
}
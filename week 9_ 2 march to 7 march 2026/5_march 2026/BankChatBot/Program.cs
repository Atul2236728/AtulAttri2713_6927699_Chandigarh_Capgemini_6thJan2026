using System;
using System.Collections.Generic;

public interface IBankAccountOperation
{
    void Deposit(decimal amount);
    void Withdraw(decimal amount);
    decimal ProcessOperation(string message);
}

class BankOperations : IBankAccountOperation
{
    decimal balance = 0;

    public void Deposit(decimal amount)
    {
        balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (balance >= amount)
            balance -= amount;
    }

    public decimal ProcessOperation(string message)
    {
        message = message.ToLower();

        string[] words = message.Split(' ');

        decimal amount = 0;

        foreach (var w in words)
        {
            if (decimal.TryParse(w, out amount))
                break;
        }

        if (message.Contains("deposit") || message.Contains("put") ||
            message.Contains("invest") || message.Contains("transfer"))
        {
            Deposit(amount);
        }
        else if (message.Contains("withdraw") || message.Contains("pull"))
        {
            Withdraw(amount);
        }

        return balance;
    }
}

class Solution
{
    static void Main()
    {
        IBankAccountOperation bank = new BankOperations();

        Console.Write("Enter number of commands: ");
        int n = int.Parse(Console.ReadLine());

        for (int i = 0; i < n; i++)
        {
            Console.Write("Enter command: ");
            string message = Console.ReadLine();

            decimal result = bank.ProcessOperation(message);

            Console.WriteLine(result);
        }
    }
}
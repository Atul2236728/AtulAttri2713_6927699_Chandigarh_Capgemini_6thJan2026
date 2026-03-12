using System;
using System.Collections.Generic;
using System.Text;

namespace bank_management_sys
{
    
    
        class BankAccount
        {
            public string Name;
            public int AccountNumber;
            public double Balance;

            public BankAccount(string name, int accNo, double balance)
            {
                Name = name;
                AccountNumber = accNo;
                Balance = balance;
            }

            public void Deposit(double amount)
            {
                Balance += amount;
                Console.WriteLine("Money Deposited Successfully.");
            }

            public void Withdraw(double amount)
            {
                if (amount > Balance)
                    Console.WriteLine("Insufficient Balance.");
                else
                {
                    Balance -= amount;
                    Console.WriteLine("Money Withdrawn Successfully.");
                }
            }

            public void Display()
            {
                Console.WriteLine("\nAccount Holder: " + Name);
                Console.WriteLine("Account Number: " + AccountNumber);
                Console.WriteLine("Balance: ₹" + Balance);
            }
        }
    }


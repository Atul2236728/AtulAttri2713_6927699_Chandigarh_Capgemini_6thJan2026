using System;
using System.Data;
using Microsoft.Data.SqlClient;
namespace LibraryDB
{
    internal class Program
    {
       
       
            static string connectionString =
                "Data Source=.\\SQLEXPRESS;Initial Catalog=LibraryDB;Integrated Security=True;TrustServerCertificate=True";

            static void Main(string[] args)
            {
                while (true)
                {
                    Console.WriteLine("\n===== LIBRARY MANAGEMENT =====");
                    Console.WriteLine("1. View Books");
                    Console.WriteLine("2. Add Book");
                    Console.WriteLine("3. Update Availability");
                    Console.WriteLine("4. Exit");

                    Console.Write("Choose option: ");
                    int choice = Convert.ToInt32(Console.ReadLine());

                    switch (choice)
                    {
                        case 1: ViewBooks(); break;
                        case 2: AddBook(); break;
                        case 3: UpdateAvailability(); break;
                        case 4: return;
                        default: Console.WriteLine("Invalid choice"); break;
                    }
                }
            }

            // ===== VIEW BOOKS (Disconnected) =====
            static void ViewBooks()
            {
                SqlDataAdapter adapter =
                    new SqlDataAdapter("SELECT * FROM Books", connectionString);

                DataSet ds = new DataSet();
                adapter.Fill(ds, "Books");

                foreach (DataRow row in ds.Tables["Books"].Rows)
                {
                    Console.WriteLine(
                        $"{row["BookId"]} - {row["Title"]} - {row["Author"]} - Available: {row["Available"]}"
                    );
                }
            }

            // ===== ADD BOOK =====
            static void AddBook()
            {
                Console.Write("Title: ");
                string title = Console.ReadLine();

                Console.Write("Author: ");
                string author = Console.ReadLine();

                SqlDataAdapter adapter =
                    new SqlDataAdapter("SELECT * FROM Books", connectionString);

                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

                DataSet ds = new DataSet();
                adapter.Fill(ds, "Books");

                DataRow newRow = ds.Tables["Books"].NewRow();
                newRow["Title"] = title;
                newRow["Author"] = author;
                newRow["Available"] = true;

                ds.Tables["Books"].Rows.Add(newRow);

                adapter.Update(ds, "Books");

                Console.WriteLine("Book added successfully.");
            }

            // ===== UPDATE AVAILABILITY =====
            static void UpdateAvailability()
            {
                Console.Write("Book ID: ");
                int id = Convert.ToInt32(Console.ReadLine());

                Console.Write("Available (true/false): ");
                bool available = Convert.ToBoolean(Console.ReadLine());

                SqlDataAdapter adapter =
                    new SqlDataAdapter("SELECT * FROM Books", connectionString);

                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

                DataSet ds = new DataSet();
                adapter.Fill(ds, "Books");

                foreach (DataRow row in ds.Tables["Books"].Rows)
                {
                    if ((int)row["BookId"] == id)
                    {
                        row["Available"] = available;
                        break;
                    }
                }

                adapter.Update(ds, "Books");

                Console.WriteLine("Availability updated.");
            }
        }
    }


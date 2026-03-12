using System.Data;
using Microsoft.Data.SqlClient;
using System;
namespace UniversityManagementSys
{
    internal class Program
    {
        static string connectionString =
            "Data Source=.\\SQLEXPRESS;Initial Catalog=UniversityDB;Integrated Security=True;TrustServerCertificate=True";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n===== UNIVERSITY MANAGEMENT =====");
                Console.WriteLine("1. Insert Student");
                Console.WriteLine("2. View All Students");
                Console.WriteLine("3. View Student By ID");
                Console.WriteLine("4. Update Student");
                Console.WriteLine("5. Delete Student");
                Console.WriteLine("6. Exit");

                Console.Write("Choose option: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1: InsertStudent(); break;
                    case 2: GetAllStudents(); break;
                    case 3: GetStudentById(); break;
                    case 4: UpdateStudent(); break;
                    case 5: DeleteStudent(); break;
                    case 6: return;
                    default: Console.WriteLine("Invalid choice"); break;
                }
            }
        }

        // INSERT
        static void InsertStudent()
        {
            Console.Write("First Name: ");
            string fn = Console.ReadLine();

            Console.Write("Last Name: ");
            string ln = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("DeptId: ");
            int deptId = Convert.ToInt32(Console.ReadLine());

            using SqlConnection con = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand("InsertStudent", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FirstName", fn);
            cmd.Parameters.AddWithValue("@LastName", ln);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@DeptId", deptId);

            con.Open();
            cmd.ExecuteNonQuery();

            Console.WriteLine("Student inserted successfully.");
        }

        // SELECT ALL
        static void GetAllStudents()
        {
            using SqlConnection con = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand("GetAllStudents", con);

            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(
                    $"{reader["StudentId"]} - " +
                    $"{reader["FirstName"]} {reader["LastName"]} - " +
                    $"{reader["Email"]} - Dept: {reader["DeptName"]}"
                );
            }
        }

        // SELECT BY ID
        static void GetStudentById()
        {
            Console.Write("Enter Student ID: ");
            int id = Convert.ToInt32(Console.ReadLine());

            using SqlConnection con = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand("GetStudentById", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StudentId", id);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                Console.WriteLine(
                    $"{reader["StudentId"]} - " +
                    $"{reader["FirstName"]} {reader["LastName"]} - " +
                    $"{reader["Email"]}"
                );
            }
            else
                Console.WriteLine("Student not found.");
        }

        // UPDATE
        static void UpdateStudent()
        {
            Console.Write("Student ID: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.Write("New First Name: ");
            string fn = Console.ReadLine();

            Console.Write("New Last Name: ");
            string ln = Console.ReadLine();

            Console.Write("New Email: ");
            string email = Console.ReadLine();

            Console.Write("New DeptId: ");
            int deptId = Convert.ToInt32(Console.ReadLine());

            using SqlConnection con = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand("UpdateStudent", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@StudentId", id);
            cmd.Parameters.AddWithValue("@FirstName", fn);
            cmd.Parameters.AddWithValue("@LastName", ln);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@DeptId", deptId);

            con.Open();
            cmd.ExecuteNonQuery();

            Console.WriteLine("Student updated successfully.");
        }

        // DELETE
        static void DeleteStudent()
        {
            Console.Write("Enter Student ID to delete: ");
            int id = Convert.ToInt32(Console.ReadLine());

            using SqlConnection con = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand("DeleteStudent", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StudentId", id);

            con.Open();
            cmd.ExecuteNonQuery();

            Console.WriteLine("Student deleted successfully.");
        }
    }
}
    


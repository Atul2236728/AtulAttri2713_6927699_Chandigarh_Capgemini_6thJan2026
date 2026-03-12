namespace grade
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int marks;
            char grade;

            Console.Write("Enter student marks (0 - 100): ");
            marks = Convert.ToInt32(Console.ReadLine());

            if (marks >= 90 && marks <= 100)
                grade = 'A';
            else if (marks >= 80)
                grade = 'B';
            else if (marks >= 70)
                grade = 'C';
            else if (marks >= 60)
                grade = 'D';
            else if (marks >= 0)
                grade = 'F';
            else
            {
                Console.WriteLine("Invalid marks entered!");
                return;
            }

            Console.WriteLine("Grade: " + grade);

        }
    }
}

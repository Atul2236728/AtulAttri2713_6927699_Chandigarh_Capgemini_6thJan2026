namespace student_grading
{
    class StudentGradingApp
    {
        static void Main()
        {
            Dictionary<int, int> studentGrades = new Dictionary<int, int>
        {
            {101, 78},
            {102, 45},
            {103, 88},
            {104, 35}
        };

            Func<double> calculateAverage =
                () => studentGrades.Values.Average();

            Console.WriteLine($"Average Grade: {calculateAverage()}");

            int threshold = 40;
            Predicate<int> isAtRisk = grade => grade < threshold;

            Console.WriteLine("\nAt-Risk Students:");
            foreach (var student in studentGrades)
            {
                if (isAtRisk(student.Value))
                    Console.WriteLine($"Roll No: {student.Key}, Grade: {student.Value}");
            }

            studentGrades[102] = 60;

            Console.WriteLine("\nAfter Grade Update:");
            foreach (var student in studentGrades)
            {
                string status = isAtRisk(student.Value) ? "At Risk" : "Safe";
                Console.WriteLine($"Roll No: {student.Key}, Grade: {student.Value}, Status: {status}");
            }
        }
    }
}

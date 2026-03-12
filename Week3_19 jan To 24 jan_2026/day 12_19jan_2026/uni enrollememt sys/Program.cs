namespace uni_enrollememt_sys
{
    class Program
    {
        static void Main()
        {
            Student s = new Student("Atul", 101, "CSE");
            Professor p = new Professor("Dr. Rao", 201, "AI");

            s.Display();
            p.Display();
        }
    }
}

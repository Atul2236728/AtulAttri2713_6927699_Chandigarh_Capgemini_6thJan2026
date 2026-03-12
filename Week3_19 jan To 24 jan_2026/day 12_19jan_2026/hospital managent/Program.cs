using System.Numerics;

namespace hospital_managent
{
    class Program
    {
        static void Main()
        {
            Patient p = new Patient("Atul", "Fever");
            Doctor d = new Doctor("Dr Sharma");

            Appointment a = new Appointment(p, d);
            a.Show();
        }
    }
}

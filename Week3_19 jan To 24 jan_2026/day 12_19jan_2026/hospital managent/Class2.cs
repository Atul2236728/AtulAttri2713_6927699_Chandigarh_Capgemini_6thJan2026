using System;
using System.Collections.Generic;
using System.Text;

namespace hospital_managent
{
    class Doctor : Person
    {
        public Doctor(string n) : base(n) { }
    }

    class Appointment
    {
        public Patient P;
        public Doctor D;
        public Appointment(Patient p, Doctor d)
        {
            P = p;
            D = d;
        }

        public void Show()
        {
            Console.WriteLine($"{P.Name} appointment with {D.Name}");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace hospital_managent
{
    class Person
    {
        public string Name;
        public Person(string n) { Name = n; }
    }

    class Patient : Person
    {
        public string Disease;
        public Patient(string n, string d) : base(n) { Disease = d; }
    }
}

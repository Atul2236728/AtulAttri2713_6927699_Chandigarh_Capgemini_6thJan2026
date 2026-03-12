using System;
using System.Collections.Generic;
using System.Text;

namespace uni_enrollememt_sys
{
    class Student : Person
    {
        public string Course;

        public Student(string name, int id, string course) : base(name, id)
        {
            Course = course;
        }

        public override void Display()
        {
            base.Display();
            Console.WriteLine("Course: " + Course);
        }
    }
}

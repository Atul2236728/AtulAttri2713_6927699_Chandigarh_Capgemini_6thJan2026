using System;
using System.Collections.Generic;
using System.Text;

namespace uni_enrollememt_sys
{

    class Professor : Person
    {
        public string Subject;

        public Professor(string name, int id, string subject) : base(name, id)
        {
            Subject = subject;
        }

        public override void Display()
        {
            base.Display();
            Console.WriteLine("Subject: " + Subject);
        }
    }
}

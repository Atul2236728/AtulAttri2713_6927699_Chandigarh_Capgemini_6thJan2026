using System;
using System.Collections.Generic;
using System.Text;

namespace uni_enrollememt_sys
{
    class Person
    {
        public string Name;
        public int Id;

        public Person(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public virtual void Display()
        {
            Console.WriteLine($"Name: {Name}, ID: {Id}");
        }
    }
}

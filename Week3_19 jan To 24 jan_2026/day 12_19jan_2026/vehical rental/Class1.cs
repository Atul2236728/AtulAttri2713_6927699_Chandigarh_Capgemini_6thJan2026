using System;
using System.Collections.Generic;
using System.Text;

namespace vehical_rental
{
    class Vehicle
    {
        public string Model;
        public int Rate;

        public Vehicle(string model, int rate)
        {
            Model = model;
            Rate = rate;
        }

        public virtual int Calculate(int days)
        {
            return Rate * days;
        }
    }
}

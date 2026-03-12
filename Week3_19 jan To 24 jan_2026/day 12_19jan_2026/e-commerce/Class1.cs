using System;
using System.Collections.Generic;
using System.Text;

namespace e_commerce
{
    class Product
    {
        protected string name;
        protected int price;
        protected int stock;

        public Product(string n, int p, int s)
        {
            name = n;
            price = p;
            stock = s;
        }

        public string Name => name;
        public int Price => price;
        public int Stock => stock;

        public void ReduceStock() { stock--; }
    }
}

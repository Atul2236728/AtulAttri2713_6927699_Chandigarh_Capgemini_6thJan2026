using System;
using System.Collections.Generic;
using System.Text;

namespace e_commerce
{
    class Cart
    {
        Product[] items = new Product[10];
        int count = 0;

        public void Add(Product p)
        {
            if (p.Stock > 0)
            {
                items[count++] = p;
                p.ReduceStock();
            }
            else Console.WriteLine("Out of Stock");
        }

        public void Show()
        {
            int total = 0;
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(items[i].Name + " ₹" + items[i].Price);
                total += items[i].Price;
            }
            Console.WriteLine("Total ₹" + total);
        }
    }
}

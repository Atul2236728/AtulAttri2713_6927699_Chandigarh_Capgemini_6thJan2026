using System;
using System.Collections.Generic;
using System.Text;

namespace e_commerce
{
    class Electronics : Product
    {
        public Electronics(string n, int p, int s) : base(n, p, s) { }
    }

    class Books : Product
    {
        public Books(string n, int p, int s) : base(n, p, s) { }
    }
}

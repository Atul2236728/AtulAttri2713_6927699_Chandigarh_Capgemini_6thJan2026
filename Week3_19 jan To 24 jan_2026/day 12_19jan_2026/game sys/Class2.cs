using System;
using System.Collections.Generic;
using System.Text;

namespace game_sys
{
    class Warrior : Character
    {
        public Warrior(string n) : base(n) { }
        public override void Attack()
        {
            Console.WriteLine(name + " sword attack");
        }
    }
}

using System.Threading;

namespace game_sys
{
    class Program
    {
        static void Main()
        {
            Warrior w = new Warrior("Thor");
            w.AddSkill("Slash");
            w.AddSkill("Shield");

            w.Attack();
            w.ShowSkills();
        }
    }
}

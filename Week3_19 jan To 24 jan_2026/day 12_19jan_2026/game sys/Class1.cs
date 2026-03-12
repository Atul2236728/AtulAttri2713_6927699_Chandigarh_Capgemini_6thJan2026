using System;
using System.Collections.Generic;
using System.Text;

namespace game_sys
{
    class Character
    {
        protected string name;
        protected int health = 100;
        protected string[] skills = new string[5];
        protected int skillCount = 0;

        public Character(string n) { name = n; }

        public void AddSkill(string s)
        {
            skills[skillCount++] = s;
        }

        public virtual void Attack()
        {
            Console.WriteLine(name + " attacks");
        }

        public void ShowSkills()
        {
            for (int i = 0; i < skillCount; i++)
                Console.WriteLine("Skill: " + skills[i]);
        }
    }
}

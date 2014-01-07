using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EvercraftKata
{
    class Rogue : Character
    {
        public Rogue()
        {
            CritModifier = 3;
        }

        public override bool Attack(int dieRoll, Character enemy)
        {
            if (dieRoll + Attributes["Dexterity"].AttributeModifier + AttackModifier >=
                enemy.ArmorClass - enemy.Attributes["Dexterity"].AttributeModifier) 
            {
                DealsDamage(dieRoll, enemy);
                GainExperience(10);
                return true;
            }
            return false;
        }
    }
}

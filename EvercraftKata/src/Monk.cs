using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvercraftKata
{
    class Monk : Character
    {
        protected override void LevelUp()
        {
            RunningExperiencePoints = 0;
            HitPoints += 6 + Attributes["Constitution"].AttributeModifier;
            Level = Math.Floor(ExperiencePoints / 1000) + 1;
            if ((int)Level%3 == 0 || (int)Level%2 == 0)
                AttackModifier++;
        }

        protected virtual double DetermineDamage(int dieRoll)
        {
            var damage = 3 + Attributes["Strength"].AttributeModifier;
            if (dieRoll == 20)
            {
                damage = (int)CritModifier * (1 + (2 * Attributes["Strength"].AttributeModifier));
            }
            if (damage < 1)
                damage = 1;
            return damage;
        }

        protected virtual void ApplyModifiers()
        {
            ArmorClass += Attributes["Dexterity"].AttributeModifier;
            HitPoints += Attributes["Constitution"].AttributeModifier;
            if (HitPoints < 1)
                HitPoints = 1;
        }
    }
}
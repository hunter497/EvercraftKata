using System;

namespace EvercraftKata
{
    class Fighter : Character
    {
        protected override void LevelUp()
        {
            RunningExperiencePoints = 0;
            HitPoints += 10 + Attributes["Constitution"].AttributeModifier;
            Level = Math.Floor(ExperiencePoints / 1000) + 1;
            AttackModifier++;
        }
    }
}

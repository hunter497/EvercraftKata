using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using NUnit.Framework.Constraints;

namespace EvercraftKata
{ 
    public class Character
    {
        public String Name { get; set; }
 
        public double ExperiencePoints { get; set; }
        public double RunningExperiencePoints { get; set; }
        public double Level { get; set; }

        public double ArmorClass { get; set; }
        public double HitPoints { get; set; }
        public Boolean IsAlive { get; set; }

        public double AttackModifier { get; set; }
        public double CritModifier { get; set; }
        
        public Boolean AttributesSet { get; set; }
        public Dictionary<String, AttributePair> Attributes { get; set; }
        public String Alignment
        {
            get { return _alignment; }

            set
            {
                if (value == "Good" || value == "Evil" || value == "Neutral")
                    _alignment = value;
                else
                {
                    throw new Exception("you're an idiot");
                }
            }
        }

        public Character()
        {
            ExperiencePoints = 0;
            Level = 1;
            Alignment = "Neutral";
            ArmorClass = 10;
            HitPoints = 5;
            IsAlive = true;
            AttackModifier = 0;
            CritModifier = 2;
            Attributes = new Dictionary<string, AttributePair>()
            {
                {"Strength", new AttributePair(this) {AttributeScore = 10, AttributeModifier = 0}},
                {"Dexterity", new AttributePair(this) {AttributeScore = 10, AttributeModifier = 0}},
                {"Constitution", new AttributePair(this) {AttributeScore = 10, AttributeModifier = 0}},
                {"Wisdom", new AttributePair(this) {AttributeScore = 10, AttributeModifier = 0}},
                {"Intelligence", new AttributePair(this) {AttributeScore = 10, AttributeModifier = 0}},
                {"Charisma", new AttributePair(this) {AttributeScore = 10, AttributeModifier = 0}}
            };
            AttributesSet = true;
        }

        public virtual bool Attack(int dieRoll, Character enemy)
        {
            if (dieRoll + Attributes["Strength"].AttributeModifier + AttackModifier >= enemy.ArmorClass)
            {
                DealsDamage(dieRoll, enemy);
                GainExperience(10);
                return true;
            }
            return false;

        }

        public void DealsDamage(int dieRoll, Character enemy)
        {
            var damageDealt = DetermineDamage(dieRoll);
            enemy.HitPoints -= damageDealt;

            if (enemy.HitPoints <= 0)
                enemy.IsAlive = false;
        }

        public void GainExperience(double points)
        {
            ExperiencePoints += points;
            RunningExperiencePoints += points;

            if ((int)RunningExperiencePoints % 1000 == 0)
            {
                LevelUp();
            }
                
        }

        protected virtual double DetermineDamage(int dieRoll)
        {
            var damage = 1 + Attributes["Strength"].AttributeModifier;
            if (dieRoll == 20)
            {
                damage = (int)CritModifier * (1 + (2 * Attributes["Strength"].AttributeModifier));
            }
            if (damage < 1)
                damage = 1;
            return damage;
        }

        protected virtual void LevelUp()
        {
            RunningExperiencePoints = 0;
            HitPoints += 5 + Attributes["Constitution"].AttributeModifier;
            Level = Math.Floor(ExperiencePoints / 1000) + 1;
            if ((int)Level % 2 == 0)
                AttackModifier++;
        }

        private String _alignment;
        private Dictionary<String, AttributePair> _attributes;

        public class AttributePair
        {
            private Character character;

            public AttributePair(Character character)
            {
                this.character = character;
            }

            public double AttributeScore
            {
                get { return _attributeScore; }

                set
                {
                    _attributeScore = value;
                    if (value < 2)
                        AttributeModifier = -5;
                    else if (value < 4)
                        AttributeModifier = -4;
                    else if (value < 6)
                        AttributeModifier = -3;
                    else if (value < 8)
                        AttributeModifier = -2;
                    else if (value < 10)
                        AttributeModifier = -1;
                    else if (value < 12)
                        AttributeModifier = 0;
                    else if (value < 14)
                        AttributeModifier = 1;
                    else if (value < 16)
                        AttributeModifier = 2;
                    else if (value < 18)
                        AttributeModifier = 3;
                    else if (value < 20)
                        AttributeModifier = 4;
                    else
                        AttributeModifier = 5;

                    if (character.AttributesSet)
                    {
                        ApplyModifiers();
                    }

                }
            }

            private void ApplyModifiers()
            {
                character.ArmorClass += character.Attributes["Dexterity"].AttributeModifier;
                character.HitPoints += character.Attributes["Constitution"].AttributeModifier;
                if (character.HitPoints < 1)
                    character.HitPoints = 1;
            }


            public int AttributeModifier { get; set; }

            private double _attributeScore;
        }




    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace EvercraftKata.tests
{
    [TestFixture]
    public sealed class CharacterTests
    {
        private Character _character;

        [SetUp]
        public void Init()
        {
            _character = new Character();
        }

        [Test]
        public void GetName()
        {
            _character.Name = "Nick";
            Assert.AreEqual("Nick", _character.Name);
        }

        [Test]
        public void GetAlignment()
        {
            Assert.IsNotNull(_character.Alignment);
        }

        [Test]
        public void SetAlignment()
        {
            _character.Alignment = "Neutral";
            Assert.AreEqual("Neutral", _character.Alignment);
        }


        [Test]
        public void AlignmentCantBeAnythingOtherThanGoodEvilNeutral()
        {
            Assert.Throws<Exception>((() => _character.Alignment = "something"));
        }

        [Test]
        public void ArmorClassShouldDefaultTo10()
        {
            Assert.AreEqual(10, _character.ArmorClass);
        }

        [Test]
        public void HitPointsShouldDefaultTo5()
        {
            Assert.AreEqual(5, _character.HitPoints);
        }

        [Test]
        public void CharacterCanHitOpponentWithLowerArmorClass()
        {
            var enemy = new Character();
            Assert.True(_character.Attack(20, enemy));
        }

        [Test]
        public void CharacterCanMissOpponentWithHigherArmorClass()
        {
            var enemy = new Character();
            Assert.False(_character.Attack(8, enemy));
        }

        [Test]
        public void CharacterCanBeDamaged()
        {
            var enemy = new Character();
            _character.DealsDamage(15,enemy);
            Assert.AreEqual(4, enemy.HitPoints);
        }

        [Test]
        public void CharacterCanBeDamagedCriticallee()
        {
            var enemy = new Character();
            _character.DealsDamage(20, enemy);
            Assert.AreEqual(3, enemy.HitPoints);
        }

        [Test]
        public void EnemyCanBeKilled()
        {
            var enemy = new Character();
            enemy.HitPoints = 1;
            _character.DealsDamage(5, enemy);
            Assert.False(enemy.IsAlive);
        }

        [Test]
        public void CharacterShouldHaveDefaultAbilities()
        {
            Assert.AreEqual(10, _character.Attributes["Strength"].AttributeScore);
            Assert.AreEqual(10, _character.Attributes["Dexterity"].AttributeScore);
            Assert.AreEqual(10, _character.Attributes["Constitution"].AttributeScore);
            Assert.AreEqual(10, _character.Attributes["Wisdom"].AttributeScore);
            Assert.AreEqual(10, _character.Attributes["Intelligence"].AttributeScore);
            Assert.AreEqual(10, _character.Attributes["Charisma"].AttributeScore);
        }

        [Test]
        public void StrengthModifierShouldBeOne()
        {
            _character.Attributes["Strength"].AttributeScore = 12;
            Assert.AreEqual(1, _character.Attributes["Strength"].AttributeModifier);
        }

        [Test]
        public void StrengthModifierShouldBeAddedToDiceRoll()
        {
            var enemy = new Character();
            enemy.ArmorClass = 3;
            _character.Attributes["Strength"].AttributeScore = 12;
            _character.Attack(2, enemy);
            Assert.AreEqual(3, enemy.HitPoints);
        }

        [Test]
        public void DoubleStrengthModifierOnCriticalHit()
        {
            var enemy = new Character();
            enemy.HitPoints = 7;
            _character.Attributes["Strength"].AttributeScore = 12;
            _character.Attack(20, enemy);
            Assert.AreEqual(1, enemy.HitPoints);
        }

        [Test]
        public void MinimumDamageShouldBe1()
        {
            var enemy = new Character();
            enemy.HitPoints = 7;
            enemy.ArmorClass = 1;
            _character.Attributes["Strength"].AttributeScore = 1;
            _character.Attack(7, enemy);
            Assert.AreEqual(6, enemy.HitPoints);
        }

        [Test]
        public void DexterityModifierShouldBeAddedToArmorClass()
        {
            _character.Attributes["Dexterity"].AttributeScore = 12;
            Assert.AreEqual(11, _character.ArmorClass);
        }

        [Test]
        public void ConstitutionModifierShouldBeAddedToHealth()
        {
            _character.Attributes["Constitution"].AttributeScore = 14;
            Assert.AreEqual(7, _character.HitPoints);
        }

        [Test]
        public void ConstitutionModifierShouldNotReduceHitPointsBelow0()
        {
            _character.HitPoints = 1;
            _character.Attributes["Constitution"].AttributeScore = 2;
            Assert.AreEqual(1, _character.HitPoints);
        }

        [Test]
        public void CharacterCanGainExperienceWhenAttacking()
        {
            var enemy = new Character();
            _character.ExperiencePoints = 0;
            _character.Attack(18, enemy);
            Assert.AreEqual(10, _character.ExperiencePoints);
        }

        [Test]
        public void CharacterLevelDefaultsTo1()
        {
            var enemy = new Character();
            Assert.AreEqual(1, _character.Level);
        }

        [Test]
        public void CharacterLevelsUpEveryThousandExperiencePoints()
        {
            _character.GainExperience(1000);
            Assert.AreEqual(2, _character.Level);
        }

        [Test]
        public void CharacterLevelShouldBe3WithExperienceLevel2011()
        {
            _character.GainExperience(1000);
            _character.GainExperience(1000);
            Assert.AreEqual(3, _character.Level);
        }

        [Test]
        public void TestLevelUpHealthPoints()
        {
            _character.GainExperience(1000);
            Assert.AreEqual(10, _character.HitPoints);
        }

        [Test]
        public void CharacterLevel4ShouldAdd2ToAttackRol()
        {
            var enemy1 = new Character();
            var enemy2 = new Character();
            enemy1.HitPoints = 1000000000;
            for (var i = 0; i < 410; i++)
                _character.Attack(15, enemy1);
            enemy2.ArmorClass = 11;
            _character.Attack(9, enemy2);
            Assert.AreEqual(4, enemy2.HitPoints);
        }

        [Test]
        public void FighterAttackModifierChangesEveryLevel()
        {
            var fighter = new Fighter();
            fighter.GainExperience(1000);
            fighter.GainExperience(1000);
            Assert.AreEqual(2, fighter.AttackModifier);
        }

        [Test]
        public void FighterLevel2Has15HitPoints()
        {
            var fighter = new Fighter();
            fighter.GainExperience(1000);
            Assert.AreEqual(15, fighter.HitPoints);
        }

        [Test]
        public void RogueDoesTripleDamageOnCritAttack()
        {
            var rogue = new Rogue();
            var enemy = new Fighter();
            rogue.Attack(20, enemy);
            Assert.AreEqual(2, enemy.HitPoints);
        }

        [Test]
        public void RogueIgnoresDexterityModifier()
        {
            var rogue = new Rogue();
            var enemy = new Fighter();
            enemy.Attributes["Dexterity"].AttributeScore = 14;
            rogue.Attack(10, enemy);
            Assert.AreEqual(4, enemy.HitPoints);
        }

        [Test]
        public void RogueDexterityModifierShouldBeAddedToDiceRoll()
        {
            var rogue = new Rogue();
            var enemy = new Character();
            enemy.ArmorClass = 3;
            rogue.Attributes["Dexterity"].AttributeScore = 12;
            rogue.Attack(2, enemy);
            Assert.AreEqual(4, enemy.HitPoints);
        }

        [Test]
        public void MonkGetsSixHitPointsPerLevel()
        {
            var monk = new Monk();
            monk.GainExperience(1000);
            Assert.AreEqual(11, monk.HitPoints);
        }
    }
}
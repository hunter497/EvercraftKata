using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace EvercraftKata.tests
{
    [TestFixture]
    public sealed class CharacterTests
    {
        [Test]
        public void GetName()
        {
            var character = new Character("Nick");
            Assert.AreEqual("Nick", character.getName());
        }
    }
}

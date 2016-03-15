using DungeonGen.Common;
using EncounterGen.Common;
using NUnit.Framework;

namespace DungeonGen.Tests.Unit.Common
{
    [TestFixture]
    public class ContentsTests
    {
        private Contents contents;

        [SetUp]
        public void Setup()
        {
            contents = new Contents();
        }

        [Test]
        public void ContentsInitialized()
        {
            Assert.That(contents.Encounters, Is.Empty);
            Assert.That(contents.Miscellaneous, Is.Empty);
            Assert.That(contents.Treasures, Is.Empty);
            Assert.That(contents.Traps, Is.Empty);
            Assert.That(contents.Pool, Is.Null);
        }

        [Test]
        public void IsNotEmptyIfEncounter()
        {
            contents.Encounters = new[] { new Encounter() };
            Assert.That(contents.IsEmpty, Is.False);
        }

        [Test]
        public void IsNotEmptyIfMiscellaneous()
        {
            contents.Miscellaneous = new[] { "misc" };
            Assert.That(contents.IsEmpty, Is.False);
        }

        [Test]
        public void IsNotEmptyIfTrap()
        {
            contents.Traps = new[] { new Trap() };
            Assert.That(contents.IsEmpty, Is.False);
        }

        [Test]
        public void IsNotEmptyIfTreasure()
        {
            contents.Treasures = new[] { new ContainedTreasure() };
            Assert.That(contents.IsEmpty, Is.False);
        }

        [Test]
        public void IsNotEmptyIfPool()
        {
            contents.Pool = new Pool();
            Assert.That(contents.IsEmpty, Is.False);
        }

        [Test]
        public void IsEmptyIfEmpty()
        {
            Assert.That(contents.IsEmpty, Is.True);
        }
    }
}

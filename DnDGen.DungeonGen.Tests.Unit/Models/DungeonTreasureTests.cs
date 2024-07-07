using DnDGen.DungeonGen.Models;
using NUnit.Framework;

namespace DnDGen.DungeonGen.Tests.Unit.Models
{
    [TestFixture]
    public class DungeonTreasureTests
    {
        private DungeonTreasure dungeonTreasure;

        [SetUp]
        public void Setup()
        {
            dungeonTreasure = new DungeonTreasure();
        }

        [Test]
        public void ContainedTreasureIsInitialized()
        {
            Assert.That(dungeonTreasure.Container, Is.Empty);
            Assert.That(dungeonTreasure.Treasure, Is.Not.Null);
            Assert.That(dungeonTreasure.Concealment, Is.Empty);
        }
    }
}

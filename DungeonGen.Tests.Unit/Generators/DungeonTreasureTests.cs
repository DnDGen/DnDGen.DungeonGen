using NUnit.Framework;

namespace DungeonGen.Tests.Unit.Common
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

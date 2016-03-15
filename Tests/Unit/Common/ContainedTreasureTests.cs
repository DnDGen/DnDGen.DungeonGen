using DungeonGen.Common;
using NUnit.Framework;

namespace DungeonGen.Tests.Unit.Common
{
    [TestFixture]
    public class ContainedTreasureTests
    {
        private ContainedTreasure containedTreasure;

        [SetUp]
        public void Setup()
        {
            containedTreasure = new ContainedTreasure();
        }

        [Test]
        public void ContainedTreasureIsInitialized()
        {
            Assert.That(containedTreasure.Container, Is.Empty);
            Assert.That(containedTreasure.Treasure, Is.Not.Null);
        }
    }
}

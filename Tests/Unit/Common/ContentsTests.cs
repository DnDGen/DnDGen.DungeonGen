using DungeonGen.Common;
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
            Assert.That(contents.Encounter, Is.Not.Null);
            Assert.That(contents.Miscellaneous, Is.Empty);
            Assert.That(contents.Treasure, Is.Not.Null);
            Assert.That(contents.TreasureContainer, Is.Empty);
        }
    }
}

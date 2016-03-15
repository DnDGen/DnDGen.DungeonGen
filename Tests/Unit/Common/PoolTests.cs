using DungeonGen.Common;
using NUnit.Framework;

namespace DungeonGen.Tests.Unit.Common
{
    [TestFixture]
    public class PoolTests
    {
        private Pool pool;

        [SetUp]
        public void Setup()
        {
            pool = new Pool();
        }

        [Test]
        public void PoolInitialized()
        {
            Assert.That(pool.Encounter, Is.Null);
            Assert.That(pool.Treasure, Is.Null);
            Assert.That(pool.MagicPower, Is.Empty);
        }
    }
}

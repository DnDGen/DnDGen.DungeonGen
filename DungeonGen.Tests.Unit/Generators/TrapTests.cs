using NUnit.Framework;

namespace DungeonGen.Tests.Unit.Common
{
    [TestFixture]
    public class TrapTests
    {
        private Trap trap;

        [SetUp]
        public void Setup()
        {
            trap = new Trap();
        }

        [Test]
        public void TrapIsInitialized()
        {
            Assert.That(trap.ChallengeRating, Is.EqualTo(0));
            Assert.That(trap.Description, Is.Empty);
            Assert.That(trap.SearchDC, Is.EqualTo(0));
        }
    }
}

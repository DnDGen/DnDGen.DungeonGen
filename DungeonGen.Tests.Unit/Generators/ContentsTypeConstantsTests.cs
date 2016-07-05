using NUnit.Framework;

namespace DungeonGen.Tests.Unit.Common
{
    [TestFixture]
    public class ContentsTypeConstantsTests
    {
        [TestCase(ContentsTypeConstants.Encounter, "Encounter")]
        [TestCase(ContentsTypeConstants.Lake, "Lake")]
        [TestCase(ContentsTypeConstants.Pool, "Pool")]
        [TestCase(ContentsTypeConstants.Trap, "Trap")]
        [TestCase(ContentsTypeConstants.Treasure, "Treasure")]
        public void Constant(string constant, string value)
        {
            Assert.That(constant, Is.EqualTo(value));
        }
    }
}

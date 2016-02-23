using DungeonGen.Common;
using NUnit.Framework;

namespace DungeonGen.Tests.Unit.Common
{
    [TestFixture]
    public class ContentsTypeConstantsTests
    {
        [TestCase(ContentsTypeConstants.Encounter, "Encounter")]
        [TestCase(ContentsTypeConstants.Trap, "Trap")]
        public void Constant(string constant, string value)
        {
            Assert.That(constant, Is.EqualTo(value));
        }
    }
}

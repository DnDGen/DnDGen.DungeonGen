using DungeonGen.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Unit.Tables
{
    [TestFixture]
    public class TableNameConstantsTests
    {
        [TestCase(TableNameConstants.Chambers, "Chambers")]
        [TestCase(TableNameConstants.DungeonAreaFromHall, "DungeonAreaFromHall")]
        [TestCase(TableNameConstants.DungeonAreaFromDoor, "DungeonAreaFromDoor")]
        public void Constant(string constant, string value)
        {
            Assert.That(constant, Is.EqualTo(value));
        }
    }
}

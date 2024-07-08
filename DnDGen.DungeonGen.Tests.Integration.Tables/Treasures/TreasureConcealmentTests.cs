using DnDGen.DungeonGen.Tables;
using NUnit.Framework;

namespace DnDGen.DungeonGen.Tests.Integration.Tables.Treasures
{
    [TestFixture]
    public class TreasureConcealmentTests : PercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.TreasureConcealment;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 15, "Invisibility spell")]
        [TestCase(16, 25, "Illusion")]
        [TestCase(26, 30, "In a secret place under the container")]
        [TestCase(31, 40, "In a secret compartment in the container")]
        [TestCase(41, 45, "Inside an ordinary item in plain view")]
        [TestCase(46, 50, "Disguised to appear as something else")]
        [TestCase(51, 55, "Under a heap of trash/dung")]
        [TestCase(56, 65, "Under a loose stone in the floor")]
        [TestCase(66, 75, "Behind a loose stone in the wall")]
        [TestCase(76, 85, "In a secret room nearby")]
        [TestCase(86, 100, "")]
        public override void Percentile(int lower, int upper, string content)
        {
            base.Percentile(lower, upper, content);
        }
    }
}

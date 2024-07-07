using DnDGen.DungeonGen.Models;
using DnDGen.DungeonGen.Tables;
using NUnit.Framework;

namespace DnDGen.DungeonGen.Tests.Integration.Tables.Areas.Rooms
{
    [TestFixture]
    public class PoolsTests : PercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.Pools;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 40, "")]
        [TestCase(41, 50, "Pool")]
        [TestCase(51, 60, ContentsTypeConstants.Encounter)]
        [TestCase(61, 90, ContentsTypeConstants.Encounter + "/" + ContentsTypeConstants.Treasure)]
        [TestCase(91, 100, ContentsConstants.MagicPool)]
        public override void Percentile(int lower, int upper, string content)
        {
            base.Percentile(lower, upper, content);
        }
    }
}

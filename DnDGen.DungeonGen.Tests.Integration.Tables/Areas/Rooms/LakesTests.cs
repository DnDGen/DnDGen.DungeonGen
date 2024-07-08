using DnDGen.DungeonGen.Models;
using DnDGen.DungeonGen.Tables;
using NUnit.Framework;

namespace DnDGen.DungeonGen.Tests.Integration.Tables.Areas.Rooms
{
    [TestFixture]
    public class LakesTests : PercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.Lakes;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 50, "")]
        [TestCase(51, 75, ContentsTypeConstants.Lake)]
        [TestCase(76, 90, ContentsTypeConstants.Lake + "/" + ContentsTypeConstants.Encounter)]
        [TestCase(91, 99, "Enchanted lake leading to different dimension, special temple, or map" + "/" + ContentsTypeConstants.Encounter)]
        [TestCase(100, 100, "Enchanted lake leading to different dimension, special temple, or map")]
        public override void Percentile(int lower, int upper, string content)
        {
            base.Percentile(lower, upper, content);
        }
    }
}

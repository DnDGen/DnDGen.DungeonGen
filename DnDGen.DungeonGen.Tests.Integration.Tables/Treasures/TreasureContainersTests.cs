using DnDGen.DungeonGen.Tables;
using NUnit.Framework;

namespace DnDGen.DungeonGen.Tests.Integration.Tables.Treasures
{
    [TestFixture]
    public class TreasureContainersTests : PercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.TreasureContainers;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 10, "Bags")]
        [TestCase(11, 20, "Sacks")]
        [TestCase(21, 30, "Small coffers")]
        [TestCase(31, 40, "Chests")]
        [TestCase(41, 50, "Huge chests")]
        [TestCase(51, 60, "Pottery jars")]
        [TestCase(61, 70, "Metal urns")]
        [TestCase(71, 80, "Stone containers")]
        [TestCase(81, 90, "Iron trunks")]
        [TestCase(91, 100, "")]
        public override void Percentile(int lower, int upper, string content)
        {
            base.Percentile(lower, upper, content);
        }
    }
}

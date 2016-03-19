using DungeonGen.Common;
using DungeonGen.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables.Areas.Rooms
{
    [TestFixture]
    public class CavesTests : AreaPercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.Caves;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 25, AreaTypeConstants.Cave, "", "", 40, 60)]
        [TestCase(26, 35, AreaTypeConstants.Cave, "", "", 50, 75)]
        [TestCase(36, 45, AreaTypeConstants.Cave, DescriptionConstants.DoubleCavern, "60/60", 20, 30)]
        [TestCase(46, 55, AreaTypeConstants.Cave, DescriptionConstants.DoubleCavern, "80/90/" + ContentsTypeConstants.Pool, 35, 50)]
        [TestCase(56, 70, AreaTypeConstants.Cave, "", ContentsTypeConstants.Pool, 95, 125)]
        [TestCase(71, 80, AreaTypeConstants.Cave, "", "", 125, 150)]
        [TestCase(81, 90, AreaTypeConstants.Cave, "", "", 150, 200)]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, int width)
        {
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);
        }

        [TestCase(91, 100, AreaTypeConstants.Cave, "", ContentsTypeConstants.Lake, "1d12*5+250", "1d12*5+350")]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, string length, string width)
        {
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);
        }
    }
}

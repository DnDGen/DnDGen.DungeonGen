using DungeonGen.Common;
using DungeonGen.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables.Areas.Rooms
{
    [TestFixture]
    public class ChamberExitsTests : AreaPercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.ChamberExits;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 15, AreaTypeConstants.Hall, "", "", 600, 1)]
        [TestCase(16, 30, AreaTypeConstants.Hall, "", "", 600, 2)]
        [TestCase(31, 45, AreaTypeConstants.Hall, "", "", 600, 3)]
        [TestCase(46, 60, AreaTypeConstants.Hall, "", "", 1200, 0)]
        [TestCase(61, 75, AreaTypeConstants.Hall, "", "", 1600, 0)]
        [TestCase(91, 100, AreaTypeConstants.Door, "", "", 0, 1)]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, int width)
        {
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);
        }

        [TestCase(76, 90, AreaTypeConstants.Hall, "", "", 0, "1d4")]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, string width)
        {
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);
        }
    }
}

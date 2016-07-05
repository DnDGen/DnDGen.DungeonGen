using DungeonGen.Domain.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables.Areas.Rooms
{
    [TestFixture]
    public class RoomExitsTests : AreaPercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.RoomExits;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 15, AreaTypeConstants.Door, "", "", 600, 1)]
        [TestCase(16, 30, AreaTypeConstants.Door, "", "", 600, 2)]
        [TestCase(31, 45, AreaTypeConstants.Door, "", "", 600, 3)]
        [TestCase(46, 60, AreaTypeConstants.Door, "", "", 1200, 0)]
        [TestCase(61, 75, AreaTypeConstants.Door, "", "", 1600, 0)]
        [TestCase(91, 100, AreaTypeConstants.Hall, "", "", 0, 1)]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, int width)
        {
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);
        }

        [TestCase(76, 90, AreaTypeConstants.Door, "", "", 0, "1d4")]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, string width)
        {
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);
        }
    }
}

using DnDGen.DungeonGen.Models;
using DnDGen.DungeonGen.Tables;
using NUnit.Framework;

namespace DnDGen.DungeonGen.Tests.Integration.Tables.Areas.Rooms
{
    [TestFixture]
    public class RoomsTests : AreaPercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.Rooms;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 10, AreaTypeConstants.Room, "", "", 10, 10)]
        [TestCase(11, 20, AreaTypeConstants.Room, "", "", 20, 20)]
        [TestCase(21, 30, AreaTypeConstants.Room, "", "", 30, 30)]
        [TestCase(31, 40, AreaTypeConstants.Room, "", "", 40, 40)]
        [TestCase(41, 50, AreaTypeConstants.Room, "", "", 10, 20)]
        [TestCase(51, 65, AreaTypeConstants.Room, "", "", 20, 30)]
        [TestCase(66, 75, AreaTypeConstants.Room, "", "", 20, 40)]
        [TestCase(76, 85, AreaTypeConstants.Room, "", "", 30, 40)]
        [TestCase(86, 100, AreaTypeConstants.Special, "", "", 0, 0)]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, int width)
        {
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);
        }
    }
}

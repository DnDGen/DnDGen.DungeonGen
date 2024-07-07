using DnDGen.DungeonGen.Models;
using DnDGen.DungeonGen.Tables;
using NUnit.Framework;

namespace DnDGen.DungeonGen.Tests.Integration.Tables.Areas.Rooms
{
    [TestFixture]
    public class ContentsTests : AreaPercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.Contents;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 18, "", "", ContentsTypeConstants.Encounter, 0, 0)]
        [TestCase(45, 45, "", "", ContentsTypeConstants.Encounter + "/" + ContentsTypeConstants.Treasure, 0, 0)]
        [TestCase(46, 46, "", "", ContentsTypeConstants.Encounter + "/" + ContentsTypeConstants.Trap, 0, 0)]
        [TestCase(49, 49, "", "", ContentsTypeConstants.Encounter + "/" + ContentsTypeConstants.Treasure + "/" + ContentsTypeConstants.Trap, 0, 0)]
        [TestCase(80, 80, "", "", ContentsTypeConstants.Treasure, 0, 0)]
        [TestCase(81, 81, "", "", ContentsTypeConstants.Treasure + "/" + ContentsTypeConstants.Trap, 0, 0)]
        [TestCase(82, 82, "", "", ContentsTypeConstants.Trap, 0, 0)]
        [TestCase(83, 100, "", "", "", 0, 0)]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, int width)
        {
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);
        }

        [TestCase(19, 44, "", "", ContentsTypeConstants.Encounter, "1d6-2", "1d8-4")]
        [TestCase(47, 47, "", "", ContentsTypeConstants.Encounter + "/" + ContentsTypeConstants.Treasure, "1d6-2", "1d8-4")]
        [TestCase(48, 48, "", "", ContentsTypeConstants.Encounter + "/" + ContentsTypeConstants.Trap, "1d6-2", "1d8-4")]
        [TestCase(50, 50, "", "", ContentsTypeConstants.Encounter + "/" + ContentsTypeConstants.Treasure + "/" + ContentsTypeConstants.Trap, "1d6-2", "1d8-4")]
        [TestCase(51, 76, "", "", "", "1d6-2", "1d8-4")]
        [TestCase(77, 77, "", "", ContentsTypeConstants.Treasure, "1d6-2", "1d8-4")]
        [TestCase(78, 78, "", "", ContentsTypeConstants.Trap, "1d6-2", "1d8-4")]
        [TestCase(79, 79, "", "", ContentsTypeConstants.Treasure + "/" + ContentsTypeConstants.Trap, "1d6-2", "1d8-4")]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, string length, string width)
        {
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);
        }
    }
}

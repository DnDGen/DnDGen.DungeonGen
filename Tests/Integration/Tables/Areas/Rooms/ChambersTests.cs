using DungeonGen.Common;
using DungeonGen.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables.Areas.Rooms
{
    public class ChambersTests : AreaPercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.Chambers;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 20, AreaTypeConstants.Chamber, "", "", 20, 20)]
        [TestCase(21, 30, AreaTypeConstants.Chamber, "", "", 30, 30)]
        [TestCase(31, 40, AreaTypeConstants.Chamber, "", "", 40, 40)]
        [TestCase(41, 65, AreaTypeConstants.Chamber, "", "", 20, 30)]
        [TestCase(66, 75, AreaTypeConstants.Chamber, "", "", 30, 50)]
        [TestCase(76, 85, AreaTypeConstants.Chamber, "", "", 40, 60)]
        [TestCase(86, 100, AreaTypeConstants.Special, "", "", 0, 0)]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, int width)
        {
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);
        }
    }
}

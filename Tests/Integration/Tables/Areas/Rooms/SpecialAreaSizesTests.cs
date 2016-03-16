using DungeonGen.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables.Areas.Rooms
{
    [TestFixture]
    public class SpecialAreaSizesTests : AreaPercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.SpecialAreaSizes;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 15, "", "", "", 500, 0)]
        [TestCase(16, 30, "", "", "", 900, 0)]
        [TestCase(31, 40, "", "", "", 1300, 0)]
        [TestCase(41, 50, "", "", "", 2000, 0)]
        [TestCase(51, 60, "", "", "", 2700, 0)]
        [TestCase(61, 70, "", "", "", 3400, 0)]
        [TestCase(71, 100, "", "", "", 0, 2000)]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, int width)
        {
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);
        }
    }
}

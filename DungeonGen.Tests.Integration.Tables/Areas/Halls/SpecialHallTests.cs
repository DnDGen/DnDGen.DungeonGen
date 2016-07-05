using DungeonGen.Domain.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables.Areas.Halls
{
    [TestFixture]
    public class SpecialHallTests : AreaPercentileTests
    {
        protected override string tableName
        {
            get
            {
                return string.Format(TableNameConstants.SpecialAREA, AreaTypeConstants.Hall);
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 20, AreaTypeConstants.Hall, "", "Columns down center", 30, 40)]
        [TestCase(21, 35, AreaTypeConstants.Hall, "", "Double row of columns", 30, 40)]
        [TestCase(36, 50, AreaTypeConstants.Hall, "", "Double row of columns", 30, 50)]
        [TestCase(51, 60, AreaTypeConstants.Hall, "", ContentsConstants.Gallery, 30, 50)]
        [TestCase(61, 75, AreaTypeConstants.Hall, "", ContentsConstants.Stream, 30, 10)]
        [TestCase(76, 85, AreaTypeConstants.Hall, "", ContentsConstants.River, 40, 10)]
        [TestCase(86, 90, AreaTypeConstants.Hall, "", ContentsConstants.River, 60, 10)]
        [TestCase(91, 95, AreaTypeConstants.Hall, "", ContentsConstants.River, 80, 10)]
        [TestCase(96, 100, AreaTypeConstants.Hall, "", ContentsConstants.Chasm, 40, 10)]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, int width)
        {
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);
        }
    }
}

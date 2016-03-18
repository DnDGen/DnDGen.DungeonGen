using DungeonGen.Common;
using DungeonGen.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables.Areas.Halls
{
    [TestFixture]
    public class StairsTests : AreaPercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.Stairs;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 30, AreaTypeConstants.Stairs, "", "", 1, 5)]
        [TestCase(31, 35, AreaTypeConstants.Stairs, "", "", 2, 10)]
        [TestCase(36, 40, AreaTypeConstants.Stairs, "", "", 3, 15)]
        [TestCase(41, 50, AreaTypeConstants.Stairs, "", "", -1, 0)]
        [TestCase(51, 55, AreaTypeConstants.Stairs, DescriptionConstants.Chimney, "", -1, 0)]
        [TestCase(56, 60, AreaTypeConstants.Stairs, DescriptionConstants.Chimney, "", -2, 0)]
        [TestCase(61, 80, AreaTypeConstants.Stairs, DescriptionConstants.TrapDoor, "", 1, 0)]
        [TestCase(81, 85, AreaTypeConstants.Stairs, DescriptionConstants.TrapDoor, "", 2, 0)]
        [TestCase(86, 100, AreaTypeConstants.Stairs, "", AreaTypeConstants.Chamber, 1, 0)]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, int width)
        {
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);
        }
    }
}

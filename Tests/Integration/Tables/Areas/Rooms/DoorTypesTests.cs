using DungeonGen.Common;
using DungeonGen.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables.Areas.Rooms
{
    [TestFixture]
    public class DoorTypesTests : AreaPercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.DoorTypes;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 8, AreaTypeConstants.Door, "Simple/Wooden", "", 0, 0)]
        [TestCase(9, 9, AreaTypeConstants.Door, "Simple/Wooden", ContentsTypeConstants.Trap, 0, 0)]
        [TestCase(10, 23, AreaTypeConstants.Door, "Simple/Wooden", "", 13, 0)]
        [TestCase(24, 24, AreaTypeConstants.Door, "Simple/Wooden", ContentsTypeConstants.Trap, 13, 0)]
        [TestCase(25, 29, AreaTypeConstants.Door, "Simple/Wooden", "", 0, 15)]
        [TestCase(30, 30, AreaTypeConstants.Door, "Simple/Wooden", ContentsTypeConstants.Trap, 0, 15)]
        [TestCase(31, 35, AreaTypeConstants.Door, "Good/Wooden", "", 0, 0)]
        [TestCase(36, 36, AreaTypeConstants.Door, "Good/Wooden", ContentsTypeConstants.Trap, 0, 0)]
        [TestCase(37, 44, AreaTypeConstants.Door, "Good/Wooden", "", 18, 0)]
        [TestCase(45, 45, AreaTypeConstants.Door, "Good/Wooden", ContentsTypeConstants.Trap, 18, 0)]
        [TestCase(46, 49, AreaTypeConstants.Door, "Good/Wooden", "", 0, 18)]
        [TestCase(50, 50, AreaTypeConstants.Door, "Good/Wooden", ContentsTypeConstants.Trap, 0, 18)]
        [TestCase(51, 55, AreaTypeConstants.Door, "Strong/Wooden", "", 0, 0)]
        [TestCase(56, 56, AreaTypeConstants.Door, "Strong/Wooden", ContentsTypeConstants.Trap, 0, 0)]
        [TestCase(57, 64, AreaTypeConstants.Door, "Strong/Wooden", "", 23, 0)]
        [TestCase(65, 65, AreaTypeConstants.Door, "Strong/Wooden", ContentsTypeConstants.Trap, 23, 0)]
        [TestCase(66, 69, AreaTypeConstants.Door, "Strong/Wooden", "", 0, 25)]
        [TestCase(70, 70, AreaTypeConstants.Door, "Strong/Wooden", ContentsTypeConstants.Trap, 0, 25)]
        [TestCase(71, 71, AreaTypeConstants.Door, DescriptionConstants.Stone, "", 0, 0)]
        [TestCase(72, 72, AreaTypeConstants.Door, DescriptionConstants.Stone, ContentsTypeConstants.Trap, 0, 0)]
        [TestCase(73, 75, AreaTypeConstants.Door, DescriptionConstants.Stone, "", 28, 0)]
        [TestCase(76, 76, AreaTypeConstants.Door, DescriptionConstants.Stone, ContentsTypeConstants.Trap, 28, 0)]
        [TestCase(77, 79, AreaTypeConstants.Door, DescriptionConstants.Stone, "", 0, 28)]
        [TestCase(80, 80, AreaTypeConstants.Door, DescriptionConstants.Stone, ContentsTypeConstants.Trap, 0, 28)]
        [TestCase(81, 81, AreaTypeConstants.Door, DescriptionConstants.Iron, "", 0, 0)]
        [TestCase(82, 82, AreaTypeConstants.Door, DescriptionConstants.Iron, ContentsTypeConstants.Trap, 0, 0)]
        [TestCase(83, 85, AreaTypeConstants.Door, DescriptionConstants.Iron, "", 28, 0)]
        [TestCase(86, 86, AreaTypeConstants.Door, DescriptionConstants.Iron, ContentsTypeConstants.Trap, 28, 0)]
        [TestCase(87, 89, AreaTypeConstants.Door, DescriptionConstants.Iron, "", 0, 28)]
        [TestCase(90, 90, AreaTypeConstants.Door, DescriptionConstants.Iron, ContentsTypeConstants.Trap, 0, 28)]
        [TestCase(91, 93, AreaTypeConstants.Special, DescriptionConstants.SlidesToSide, "", 1, 0)]
        [TestCase(94, 96, AreaTypeConstants.Special, DescriptionConstants.SlidesDown, "", 1, 0)]
        [TestCase(97, 99, AreaTypeConstants.Special, DescriptionConstants.SlidesUp, "", 2, 0)]
        [TestCase(100, 100, AreaTypeConstants.Special, DescriptionConstants.MagicallyReinforced, "", 30, 40)]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, int width)
        {
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);
        }
    }
}

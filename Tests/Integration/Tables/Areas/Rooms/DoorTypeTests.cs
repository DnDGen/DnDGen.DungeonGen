using DungeonGen.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables.Areas.Rooms
{
    [TestFixture]
    public class DoorTypeTests : AreaPercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.DoorType;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 8, "", "", "", 0, 0)]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, int width)
        {
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);
        }
    }
}

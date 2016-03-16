using DungeonGen.Tables;
using NUnit.Framework;
using System;

namespace DungeonGen.Tests.Integration.Tables.Areas.Rooms
{
    [TestFixture]
    public class DoorTypeTests : AreaPercentileTests
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

        [TestCase(1, 8, "", "", "", 0, 0)]
        [TestCase(9, 9, "", "", "", 0, 0)]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, int width)
        {
            throw new NotImplementedException("Need to finish writing the test cases.  Will probably dictate changes to the door generator");
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);
        }
    }
}

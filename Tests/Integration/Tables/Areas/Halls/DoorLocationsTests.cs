using DungeonGen.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables.Areas.Halls
{
    [TestFixture]
    public class DoorLocationsTests : PercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.DoorLocations;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 30, "On the left")]
        [TestCase(31, 60, "On the right")]
        [TestCase(61, 100, "Straight ahead")]
        public override void Percentile(int lower, int upper, string content)
        {
            base.Percentile(lower, upper, content);
        }
    }
}

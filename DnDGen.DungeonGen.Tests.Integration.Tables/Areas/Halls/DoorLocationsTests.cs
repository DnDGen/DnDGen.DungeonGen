using DnDGen.DungeonGen.Models;
using DnDGen.DungeonGen.Tables;
using NUnit.Framework;

namespace DnDGen.DungeonGen.Tests.Integration.Tables.Areas.Halls
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

        [TestCase(1, 30, DescriptionConstants.OnTheLeft)]
        [TestCase(31, 60, DescriptionConstants.OnTheRight)]
        [TestCase(61, 100, DescriptionConstants.StraightAhead)]
        public override void Percentile(int lower, int upper, string content)
        {
            base.Percentile(lower, upper, content);
        }
    }
}

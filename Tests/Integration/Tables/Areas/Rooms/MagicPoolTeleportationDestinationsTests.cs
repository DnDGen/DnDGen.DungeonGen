using DungeonGen.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables.Areas.Rooms
{
    [TestFixture]
    public class MagicPoolTeleportationDestinationsTests : PercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.MagicPoolTeleportationDestinations;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 35, "Back to the surface or outside")]
        [TestCase(36, 50, "Somewhere on the same level (same every time)")]
        [TestCase(51, 60, "Somewhere on the same level (different every time)")]
        [TestCase(61, 75, "Somewhere down one level (same every time)")]
        [TestCase(76, 80, "Somewhere down one level (different every time)")]
        [TestCase(81, 100, "100 miles away, outdoors")]
        public override void Percentile(int lower, int upper, string content)
        {
            base.Percentile(lower, upper, content);
        }
    }
}

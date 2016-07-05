using DungeonGen.Domain.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables.Areas.Halls
{
    [TestFixture]
    public class RiverCrossingTests : PercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.RiverCrossing;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 25, "Boat")]
        [TestCase(26, 100, "Bridge")]
        public override void Percentile(int lower, int upper, string content)
        {
            base.Percentile(lower, upper, content);
        }
    }
}

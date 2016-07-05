using DungeonGen.Domain.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables.Areas.Rooms
{
    [TestFixture]
    public class ExitDirectionTests : PercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.ExitDirection;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 80, "Straight ahead")]
        [TestCase(81, 90, "45 degrees left")]
        [TestCase(91, 100, "45 degrees right")]
        public override void Percentile(int lower, int upper, string content)
        {
            base.Percentile(lower, upper, content);
        }
    }
}

using DnDGen.DungeonGen.Tables;
using NUnit.Framework;

namespace DnDGen.DungeonGen.Tests.Integration.Tables.Areas.Rooms
{
    [TestFixture]
    public class ExitLocationTests : PercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.ExitLocation;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 35, "Opposite wall")]
        [TestCase(36, 60, "Left wall")]
        [TestCase(61, 85, "Right wall")]
        [TestCase(86, 100, "Same wall")]
        public override void Percentile(int lower, int upper, string content)
        {
            base.Percentile(lower, upper, content);
        }
    }
}

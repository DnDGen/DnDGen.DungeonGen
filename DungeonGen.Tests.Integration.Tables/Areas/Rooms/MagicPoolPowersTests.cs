using DungeonGen.Domain.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables.Areas.Rooms
{
    [TestFixture]
    public class MagicPoolPowersTests : PercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.MagicPoolPowers;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 20, "Changes gold to lead one time")]
        [TestCase(21, 40, "Changes gold to platinum one time")]
        [TestCase(41, 75, "Changes a stat (random per character, once per character: 50% raise, 50% lower, random stat (1d6), adjusted 1d3 points)")]
        [TestCase(76, 85, ContentsConstants.LikesAlignment)]
        [TestCase(86, 100, ContentsConstants.TeleportationPool)]
        public override void Percentile(int lower, int upper, string content)
        {
            base.Percentile(lower, upper, content);
        }
    }
}

using DungeonGen.Domain.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables.Areas.Halls
{
    [TestFixture]
    public class TurnsTests : PercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.Turns;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 40, SidePassageConstants.Left90Degrees)]
        [TestCase(41, 45, SidePassageConstants.Left45DegreesAhead)]
        [TestCase(46, 50, SidePassageConstants.Left45DegreesBehind)]
        [TestCase(51, 90, SidePassageConstants.Right90Degrees)]
        [TestCase(91, 95, SidePassageConstants.Right45DegreesAhead)]
        [TestCase(96, 100, SidePassageConstants.Right45DegreesBehind)]
        public override void Percentile(int lower, int upper, string content)
        {
            base.Percentile(lower, upper, content);
        }
    }
}

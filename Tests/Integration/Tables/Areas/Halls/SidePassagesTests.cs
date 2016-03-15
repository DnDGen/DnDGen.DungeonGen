using DungeonGen.Common;
using DungeonGen.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables.Areas.Halls
{
    [TestFixture]
    public class SidePassagesTests : PercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.SidePassages;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 10, SidePassageConstants.Left90Degrees)]
        [TestCase(11, 20, SidePassageConstants.Right90Degrees)]
        [TestCase(21, 30, SidePassageConstants.Left45DegreesAhead)]
        [TestCase(31, 40, SidePassageConstants.Right45DegreesAhead)]
        [TestCase(41, 45, SidePassageConstants.Left45DegreesBehind)]
        [TestCase(46, 50, SidePassageConstants.Right45DegreesBehind)]
        [TestCase(51, 65, SidePassageConstants.TIntersection)]
        [TestCase(66, 75, SidePassageConstants.YIntersection)]
        [TestCase(76, 95, SidePassageConstants.CrossIntersection)]
        [TestCase(96, 100, SidePassageConstants.XIntersection)]
        public override void Percentile(int lower, int upper, string content)
        {
            base.Percentile(lower, upper, content);
        }
    }
}

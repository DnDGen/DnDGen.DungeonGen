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

        [TestCase(SidePassageConstants.Left90Degrees, 1, 10)]
        [TestCase(SidePassageConstants.Right90Degrees, 11, 20)]
        [TestCase(SidePassageConstants.Left45DegreesAhead, 21, 30)]
        [TestCase(SidePassageConstants.Right45DegreesAhead, 31, 40)]
        [TestCase(SidePassageConstants.Left45DegreesBehind, 41, 45)]
        [TestCase(SidePassageConstants.Right45DegreesBehind, 46, 50)]
        [TestCase(SidePassageConstants.TIntersection, 51, 65)]
        [TestCase(SidePassageConstants.YIntersection, 66, 75)]
        [TestCase(SidePassageConstants.CrossIntersection, 76, 95)]
        [TestCase(SidePassageConstants.XIntersection, 96, 100)]
        public override void Percentile(string content, int lower, int upper)
        {
            base.Percentile(content, lower, upper);
        }
    }
}

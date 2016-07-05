using DungeonGen.Domain.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables.Areas.Halls
{
    [TestFixture]
    public class HallsTests : AreaPercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.Halls;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 60, AreaTypeConstants.Hall, "", "", 30, 10)]
        [TestCase(61, 80, AreaTypeConstants.Hall, "", "", 30, 20)]
        [TestCase(81, 85, AreaTypeConstants.Hall, "", "", 30, 30)]
        [TestCase(86, 90, AreaTypeConstants.Hall, "", "", 30, 5)]
        [TestCase(91, 100, AreaTypeConstants.Special, "", "", 0, 0)]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, int width)
        {
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);
        }
    }
}

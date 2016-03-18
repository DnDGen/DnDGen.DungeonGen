using DungeonGen.Common;
using DungeonGen.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables.Areas.Halls
{
    [TestFixture]
    public class AdditionalGalleryStairsTests : PercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.AdditionalGalleryStairs;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 50, "")]
        [TestCase(51, 100, ContentsConstants.GalleryStairs_End)]
        public override void Percentile(int lower, int upper, string content)
        {
            base.Percentile(lower, upper, content);
        }
    }
}

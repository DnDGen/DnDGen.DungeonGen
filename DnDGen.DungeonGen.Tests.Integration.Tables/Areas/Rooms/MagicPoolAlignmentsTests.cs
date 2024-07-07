using DnDGen.DungeonGen.Tables;
using NUnit.Framework;

namespace DnDGen.DungeonGen.Tests.Integration.Tables.Areas.Rooms
{
    [TestFixture]
    public class MagicPoolAlignmentsTests : PercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.MagicPoolAlignments;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 30, "Lawful Good")]
        [TestCase(31, 45, "Lawful Evil")]
        [TestCase(46, 60, "Chaotic Good")]
        [TestCase(61, 85, "Chaotic Evil")]
        [TestCase(86, 100, "Any neutral alignment")]
        public override void Percentile(int lower, int upper, string content)
        {
            base.Percentile(lower, upper, content);
        }
    }
}

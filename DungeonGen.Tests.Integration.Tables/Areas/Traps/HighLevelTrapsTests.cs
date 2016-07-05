using DungeonGen.Domain.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables.Areas.Traps
{
    [TestFixture]
    public class HighLevelTrapsTests : AreaPercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.HighLevelTraps;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 4, "Acid fog trap", "7", "", 31, 31)]
        [TestCase(5, 7, "Blade barrier trap", "7", "", 31, 31)]
        [TestCase(8, 10, "Burnt othur vapor trap", "7", "", 21, 21)]
        [TestCase(11, 14, "Chain lightning trap", "7", "", 31, 31)]
        [TestCase(15, 17, "Evard's black tentacles trap", "7", "", 29, 29)]
        [TestCase(18, 20, "Fusillade of greenblood oil darts", "7", "", 25, 25)]
        [TestCase(21, 23, "Lock covered in dragon bile", "7", "", 27, 16)]
        [TestCase(24, 26, "Summon monster VI trap", "7", "", 31, 31)]
        [TestCase(27, 30, "Water-filled room", "7", "", 20, 25)]
        [TestCase(31, 33, "Well-camouflaged pit trap", "7", "", 27, 18)]
        [TestCase(34, 36, "Deathblade wall scythe", "8", "", 24, 19)]
        [TestCase(37, 39, "Destruction trap", "8", "", 32, 32)]
        [TestCase(40, 42, "Earthquake trap", "8", "", 32, 32)]
        [TestCase(43, 46, "Insanity mist vapor trap", "8", "", 25, 20)]
        [TestCase(47, 49, "Melf's acid arrow trap", "8", "", 27, 27)]
        [TestCase(50, 52, "Power word stun trap", "8", "", 32, 32)]
        [TestCase(53, 55, "Prismatic spray trap", "8", "", 32, 32)]
        [TestCase(56, 59, "Reverse gravity trap", "8", "", 32, 32)]
        [TestCase(60, 62, "Well-camouflaged pit trap", "8", "", 27, 18)]
        [TestCase(63, 65, "Word of chaos trap", "8", "", 32, 32)]
        [TestCase(66, 68, "Drawer handle smeared with contact poison", "9", "", 18, 26)]
        [TestCase(69, 71, "Dropping ceiling", "9", "", 20, 16)]
        [TestCase(72, 74, "Incendiary cloud trap", "9", "", 33, 33)]
        [TestCase(75, 77, "Wide-mouth pit trap", "9", "", 25, 25)]
        [TestCase(78, 80, "Wide mouth spiked pit with poisoned spikes", "9", "", 20, 20)]
        [TestCase(81, 84, "Crushing room", "10", "", 22, 20)]
        [TestCase(85, 88, "Crushing wall trap", "10", "", 20, 25)]
        [TestCase(89, 91, "Energy drain trap", "10", "", 34, 34)]
        [TestCase(92, 94, "Forcecage and summon monster VII trap", "10", "", 32, 32)]
        [TestCase(95, 97, "Poisoned spiked pit trap", "10", "", 16, 25)]
        [TestCase(98, 100, "Wail of the banshee trap", "10", "", 34, 34)]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, int width)
        {
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);
        }
    }
}

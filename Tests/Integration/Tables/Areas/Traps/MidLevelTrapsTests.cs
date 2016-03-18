using DungeonGen.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables.Areas.Traps
{
    [TestFixture]
    public class MidLevelTrapsTests : AreaPercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.MidLevelTraps;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 2, "Bestow curse trap", "4", "", 28, 28)]
        [TestCase(3, 5, "Camouflaged pit trap", "4", "", 25, 17)]
        [TestCase(6, 7, "Collapsing column", "4", "", 20, 24)]
        [TestCase(8, 10, "Glyph of warding, blast", "4", "", 28, 28)]
        [TestCase(11, 12, "Lightning bolt trap", "4", "", 28, 28)]
        [TestCase(13, 15, "Pit trap", "4", "", 20, 20)]
        [TestCase(16, 18, "Poisoned dart trap", "4", "", 21, 22)]
        [TestCase(19, 21, "Sepia snake sigil trap", "4", "", 28, 28)]
        [TestCase(22, 24, "Spiked pit trap", "4", "", 20, 20)]
        [TestCase(25, 26, "Wall scythe trap", "4", "", 21, 18)]
        [TestCase(27, 29, "Water-filled room trap", "4", "", 17, 23)]
        [TestCase(30, 32, "Wide-mouth spiked pit trap", "4", "", 18, 25)]
        [TestCase(33, 35, "Camouflaged pit trap", "5", "", 25, 17)]
        [TestCase(36, 37, "Doorknob smeared with contact poison", "5", "", 25, 19)]
        [TestCase(38, 40, "Falling block trap", "5", "", 20, 25)]
        [TestCase(41, 43, "Fire trap", "5", "", 29, 29)]
        [TestCase(44, 46, "Fireball trap", "5", "", 28, 28)]
        [TestCase(47, 48, "Flooding room trap", "5", "", 20, 25)]
        [TestCase(49, 51, "Fusillade of darts", "5", "", 19, 25)]
        [TestCase(52, 53, "Moving executioner statue", "5", "", 25, 18)]
        [TestCase(54, 55, "Phantasmal killer trap", "5", "", 29, 29)]
        [TestCase(56, 58, "Pit trap", "5", "", 20, 20)]
        [TestCase(59, 61, "Poison wall spikes", "5", "", 17, 21)]
        [TestCase(62, 64, "Spiked pit trap", "5", "", 21, 20)]
        [TestCase(65, 67, "Spiked pit trap, 80 ft.", "5", "", 20, 20)]
        [TestCase(68, 69, "Ungol dust vapor trap", "5", "", 20, 16)]
        [TestCase(70, 71, "Built-to-collapse wall", "6", "", 14, 16)]
        [TestCase(72, 74, "Compacting room", "6", "", 20, 22)]
        [TestCase(75, 77, "Flame strike trap", "6", "", 30, 30)]
        [TestCase(78, 80, "Fusillade of spears", "6", "", 26, 20)]
        [TestCase(81, 83, "Glyph of warding, blast", "6", "", 28, 28)]
        [TestCase(84, 85, "Lightning bolt trap", "6", "", 28, 28)]
        [TestCase(86, 88, "Spiked blocks from ceiling", "6", "", 24, 20)]
        [TestCase(89, 91, "Spiked pit trap, 100 ft.", "6", "", 20, 20)]
        [TestCase(92, 94, "Whirling poison blades", "6", "", 20, 20)]
        [TestCase(95, 97, "Wide-mouth pit trap", "6", "", 26, 25)]
        [TestCase(98, 100, "Wyvern arrow trap", "6", "", 20, 16)]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, int width)
        {
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);
        }
    }
}

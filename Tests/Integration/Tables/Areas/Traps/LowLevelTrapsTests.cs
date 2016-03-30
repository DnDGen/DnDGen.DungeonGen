using DungeonGen.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables.Areas.Traps
{
    [TestFixture]
    public class LowLevelTrapsTests : AreaPercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.LowLevelTraps;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 3, "Basic arrow trap", "1", "", 20, 20)]
        [TestCase(4, 6, "Camouflaged pit trap", "1", "", 24, 20)]
        [TestCase(7, 9, "Deeper pit trap", "1", "", 20, 23)]
        [TestCase(10, 11, "Touchable surface smeared with contact poison", "1", "", 19, 19)]
        [TestCase(12, 14, "Fusillade of darts", "1", "", 14, 20)]
        [TestCase(15, 16, "Poisoned dart trap", "1", "", 20, 18)]
        [TestCase(17, 19, "Poisoned needle trap", "1", "", 22, 20)]
        [TestCase(20, 22, "Portcullis trap", "1", "", 20, 20)]
        [TestCase(23, 24, "Razor-wire across area", "1", "", 22, 15)]
        [TestCase(25, 27, "Rolling rock trap", "1", "", 20, 22)]
        [TestCase(28, 30, "Scything blade trap", "1", "", 21, 20)]
        [TestCase(31, 33, "Spear trap", "1", "", 20, 20)]
        [TestCase(34, 35, "Swinging block trap", "1", "", 20, 20)]
        [TestCase(36, 38, "Wall blade trap", "1", "", 22, 22)]
        [TestCase(39, 41, "Box of brown mold", "2", "", 22, 16)]
        [TestCase(42, 44, "Bricks from ceiling", "2", "", 20, 20)]
        [TestCase(45, 47, "Burning hands trap", "2", "", 26, 26)]
        [TestCase(48, 50, "Camouflaged pit trap", "2", "", 24, 19)]
        [TestCase(51, 53, "Inflict light wounds trap", "2", "", 26, 26)]
        [TestCase(54, 56, "Javelin trap", "2", "", 20, 18)]
        [TestCase(57, 58, "Large net trap", "2", "", 20, 25)]
        [TestCase(59, 61, "Pit trap", "2", "", 20, 20)]
        [TestCase(62, 64, "Poison needle trap", "2", "", 22, 17)]
        [TestCase(65, 67, "Spiked pit trap", "2", "", 18, 15)]
        [TestCase(68, 69, "Tripping chain", "2", "", 15, 18)]
        [TestCase(70, 72, "Well-camouflaged pit trap", "2", "", 27, 20)]
        [TestCase(73, 75, "Burning hands trap", "3", "", 26, 26)]
        [TestCase(76, 78, "Camouflaged pit trap", "3", "", 24, 18)]
        [TestCase(79, 80, "Ceiling pendulum", "3", "", 15, 27)]
        [TestCase(81, 83, "Fire trap", "3", "", 27, 27)]
        [TestCase(84, 85, "Extended bane trap", "3", "", 27, 27)]
        [TestCase(86, 87, "Ghoul touch trap", "3", "", 27, 27)]
        [TestCase(88, 90, "Hail of needles", "3", "", 22, 22)]
        [TestCase(91, 92, "Melf's acid arrow trap", "3", "", 27, 27)]
        [TestCase(93, 95, "Pit trap", "3", "", 20, 20)]
        [TestCase(96, 98, "Spiked pit trap", "3", "", 21, 20)]
        [TestCase(99, 100, "Stone blocks from ceiling", "3", "", 25, 20)]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, int width)
        {
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);
        }
    }
}

using DnDGen.DungeonGen.Tables;
using NUnit.Framework;

namespace DnDGen.DungeonGen.Tests.Integration.Tables.Areas.Rooms
{
    [TestFixture]
    public class MinorFeaturesTests : PercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.MinorFeatures;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 1, "Anvil")]
        [TestCase(2, 2, "Ash")]
        [TestCase(3, 3, "Backpack")]
        [TestCase(4, 4, "Bale of straw")]
        [TestCase(5, 5, "Bellows")]
        [TestCase(6, 6, "Belt")]
        [TestCase(7, 7, "Bits of fur")]
        [TestCase(8, 8, "Blanket")]
        [TestCase(9, 9, "Bloodstain")]
        [TestCase(10, 10, "Humanoid bones")]
        [TestCase(11, 11, "Nonhumanoid bones")]
        [TestCase(12, 12, "Books")]
        [TestCase(13, 13, "Boots")]
        [TestCase(14, 14, "Bottle")]
        [TestCase(15, 15, "Box")]
        [TestCase(16, 16, "Branding iron")]
        [TestCase(17, 17, "Broken glass")]
        [TestCase(18, 18, "Bucket")]
        [TestCase(19, 19, "Candle")]
        [TestCase(20, 20, "Candelabra")]
        [TestCase(21, 21, "Playing cards")]
        [TestCase(22, 22, "Chains")]
        [TestCase(23, 23, "Claw marks")]
        [TestCase(24, 24, "Cleaver")]
        [TestCase(25, 25, "Clothing")]
        [TestCase(26, 26, "Cobwebs")]
        [TestCase(27, 27, "Cold spot")]
        [TestCase(28, 28, "Adventurer's corpse")]
        [TestCase(29, 29, "Monster's corpse")]
        [TestCase(30, 30, "Cracks")]
        [TestCase(31, 31, "Dice")]
        [TestCase(32, 32, "Discarded weapons")]
        [TestCase(33, 33, "Dishes")]
        [TestCase(34, 34, "Dripping water")]
        [TestCase(35, 35, "Drum")]
        [TestCase(36, 36, "Dust")]
        [TestCase(37, 37, "Engraving")]
        [TestCase(38, 38, "Broken equipment")]
        [TestCase(39, 39, "Usable equipment")]
        [TestCase(40, 40, "Flask")]
        [TestCase(41, 41, "Flint and tinder")]
        [TestCase(42, 42, "Spoiled foodstuffs")]
        [TestCase(43, 43, "Edible foodstuffs")]
        [TestCase(44, 44, "Fungus")]
        [TestCase(45, 45, "Grinder")]
        [TestCase(46, 46, "Hook")]
        [TestCase(47, 47, "Horn")]
        [TestCase(48, 48, "Hourglass")]
        [TestCase(49, 49, "Insects")]
        [TestCase(50, 50, "Jar")]
        [TestCase(51, 51, "Keg")]
        [TestCase(52, 52, "Key")]
        [TestCase(53, 53, "Lamp")]
        [TestCase(54, 54, "Lantern")]
        [TestCase(55, 55, "Markings")]
        [TestCase(56, 56, "Mold")]
        [TestCase(57, 57, "Mud")]
        [TestCase(58, 58, "Mug")]
        [TestCase(59, 59, "Musical instrument")]
        [TestCase(60, 60, "Mysterious stain")]
        [TestCase(61, 61, "Animal nest")]
        [TestCase(62, 62, "Unidentifiable odor")]
        [TestCase(63, 63, "Oil (fuel)")]
        [TestCase(64, 64, "Scented oil")]
        [TestCase(65, 65, "Paint")]
        [TestCase(66, 66, "Paper")]
        [TestCase(67, 67, "Pillows")]
        [TestCase(68, 68, "Smoking pipe")]
        [TestCase(69, 69, "Pole")]
        [TestCase(70, 70, "Pot")]
        [TestCase(71, 71, "Pottery shard")]
        [TestCase(72, 72, "Pouch")]
        [TestCase(73, 73, "Puddle of water")]
        [TestCase(74, 74, "Rags")]
        [TestCase(75, 75, "Razor")]
        [TestCase(76, 76, "Rivulet")]
        [TestCase(77, 77, "Ropes")]
        [TestCase(78, 78, "Runes")]
        [TestCase(79, 79, "Sack")]
        [TestCase(80, 80, "Scattered stones")]
        [TestCase(81, 81, "Scorch marks")]
        [TestCase(82, 82, "Nonmagical scroll")]
        [TestCase(83, 83, "Empty scroll case")]
        [TestCase(84, 84, "Skull")]
        [TestCase(85, 85, "Slime")]
        [TestCase(86, 86, "Unexplained sound")]
        [TestCase(87, 87, "Spices")]
        [TestCase(88, 88, "Spike")]
        [TestCase(89, 89, "Teeth")]
        [TestCase(90, 90, "Tongs")]
        [TestCase(91, 91, "Tools")]
        [TestCase(92, 92, "Torch stub")]
        [TestCase(93, 93, "Tray")]
        [TestCase(94, 94, "Trophy")]
        [TestCase(95, 95, "Twine")]
        [TestCase(96, 96, "Urn")]
        [TestCase(97, 97, "Utensils")]
        [TestCase(98, 98, "Whetstone")]
        [TestCase(99, 99, "Wood scraps")]
        [TestCase(100, 100, "Scrawled words")]
        public override void Percentile(int lower, int upper, string content)
        {
            base.Percentile(lower, upper, content);
        }
    }
}

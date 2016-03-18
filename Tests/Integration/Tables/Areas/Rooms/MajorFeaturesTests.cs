using DungeonGen.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables.Areas.Rooms
{
    [TestFixture]
    public class MajorFeaturesTests : PercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.MajorFeatures;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 1, "Alcove")]
        [TestCase(2, 2, "Altar")]
        [TestCase(3, 3, "Arch")]
        [TestCase(4, 4, "Arrow slit in the wall or murder hole in the ceiling")]
        [TestCase(5, 5, "Balcony")]
        [TestCase(6, 6, "Barrel")]
        [TestCase(7, 7, "Bed")]
        [TestCase(8, 8, "Bench")]
        [TestCase(9, 9, "Bookcase")]
        [TestCase(10, 10, "Brazier")]
        [TestCase(11, 11, "Cage")]
        [TestCase(12, 12, "Cauldron")]
        [TestCase(13, 13, "Carpet")]
        [TestCase(14, 14, "Carving")]
        [TestCase(15, 15, "Casket")]
        [TestCase(16, 16, "Catwalk")]
        [TestCase(17, 17, "Chair")]
        [TestCase(18, 18, "Chandelier")]
        [TestCase(19, 19, "Charcoal bin")]
        [TestCase(20, 20, "Chasm")]
        [TestCase(21, 21, "Chest")]
        [TestCase(22, 22, "Chest of drawers")]
        [TestCase(23, 23, "Chute")]
        [TestCase(24, 24, "Coat rack")]
        [TestCase(25, 25, "Collapsed wall")]
        [TestCase(26, 26, "Crate")]
        [TestCase(27, 27, "Cupboard")]
        [TestCase(28, 28, "Curtain")]
        [TestCase(29, 29, "Divan")]
        [TestCase(30, 30, "Dome")]
        [TestCase(31, 31, "Broken door")]
        [TestCase(32, 32, "Dung heap")]
        [TestCase(33, 33, "Evil symbol")]
        [TestCase(34, 34, "Fallen stones")]
        [TestCase(35, 35, "Firepit")]
        [TestCase(36, 36, "Fireplace")]
        [TestCase(37, 37, "Font")]
        [TestCase(38, 38, "Forge")]
        [TestCase(39, 39, "Fountain")]
        [TestCase(40, 40, "Broken furniture")]
        [TestCase(41, 41, "Gong")]
        [TestCase(42, 42, "Pile of hay")]
        [TestCase(43, 43, "Hole")]
        [TestCase(44, 44, "Blasted hole")]
        [TestCase(45, 45, "Idol")]
        [TestCase(46, 46, "Iron bars")]
        [TestCase(47, 47, "Iron maiden")]
        [TestCase(48, 48, "Kiln")]
        [TestCase(49, 49, "Ladder")]
        [TestCase(50, 50, "Ledge")]
        [TestCase(51, 51, "Loom")]
        [TestCase(52, 52, "Loose masonry")]
        [TestCase(53, 53, "Manacles")]
        [TestCase(54, 54, "Manger")]
        [TestCase(55, 55, "Mirror")]
        [TestCase(56, 56, "Mosaic")]
        [TestCase(57, 57, "Mound of rubble")]
        [TestCase(58, 58, "Oven")]
        [TestCase(59, 59, "Overhang")]
        [TestCase(60, 60, "Painting")]
        [TestCase(61, 61, "Partially collapsed ceiling")]
        [TestCase(62, 62, "Pedestal")]
        [TestCase(63, 63, "Peephole")]
        [TestCase(64, 64, "Pillar")]
        [TestCase(65, 65, "Pillory")]
        [TestCase(66, 66, "Shallow pit")]
        [TestCase(67, 67, "Platform")]
        [TestCase(68, 68, "Pool")]
        [TestCase(69, 69, "Portcullis")]
        [TestCase(70, 70, "Rack")]
        [TestCase(71, 71, "Ramp")]
        [TestCase(72, 72, "Recess")]
        [TestCase(73, 73, "Relief")]
        [TestCase(74, 74, "Sconce")]
        [TestCase(75, 75, "Screen")]
        [TestCase(76, 76, "Shaft")]
        [TestCase(77, 77, "Shelf")]
        [TestCase(78, 78, "Shrine")]
        [TestCase(79, 79, "Spinning wheel")]
        [TestCase(80, 80, "Stall or pen")]
        [TestCase(81, 81, "Statue")]
        [TestCase(82, 82, "Toppled statue")]
        [TestCase(83, 83, "Steps")]
        [TestCase(84, 84, "Stool")]
        [TestCase(85, 85, "Stuffed beast")]
        [TestCase(86, 86, "Sunken area")]
        [TestCase(87, 87, "Large table")]
        [TestCase(88, 88, "Small table")]
        [TestCase(89, 89, "Tapestry")]
        [TestCase(90, 90, "Throne")]
        [TestCase(91, 91, "Pile of trash")]
        [TestCase(92, 92, "Tripod")]
        [TestCase(93, 93, "Trough")]
        [TestCase(94, 94, "Tub")]
        [TestCase(95, 95, "Wall basin")]
        [TestCase(96, 96, "Wardrobe")]
        [TestCase(97, 97, "Weapon rack")]
        [TestCase(98, 98, "Well")]
        [TestCase(99, 99, "Winch and pulley")]
        [TestCase(100, 100, "Workbench")]
        public override void Percentile(int lower, int upper, string content)
        {
            base.Percentile(lower, upper, content);
        }
    }
}

using DungeonGen.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Unit.Tables
{
    [TestFixture]
    public class TableNameConstantsTests
    {
        [TestCase(TableNameConstants.AdditionalGalleryStairs, "AdditionalGalleryStairs")]
        [TestCase(TableNameConstants.ChamberExits, "ChamberExits")]
        [TestCase(TableNameConstants.Chambers, "Chambers")]
        [TestCase(TableNameConstants.ChasmCrossing, "ChasmCrossing")]
        [TestCase(TableNameConstants.Contents, "Contents")]
        [TestCase(TableNameConstants.DoorLocations, "DoorLocations")]
        [TestCase(TableNameConstants.DoorTypes, "DoorTypes")]
        [TestCase(TableNameConstants.DungeonAreaFromHall, "DungeonAreaFromHall")]
        [TestCase(TableNameConstants.DungeonAreaFromDoor, "DungeonAreaFromDoor")]
        [TestCase(TableNameConstants.ExitDirection, "ExitDirection")]
        [TestCase(TableNameConstants.ExitLocation, "ExitLocation")]
        [TestCase(TableNameConstants.GalleryStairs, "GalleryStairs")]
        [TestCase(TableNameConstants.Halls, "Halls")]
        [TestCase(TableNameConstants.MajorFeatures, "MajorFeatures")]
        [TestCase(TableNameConstants.MinorFeatures, "MinorFeatures")]
        [TestCase(TableNameConstants.RiverCrossing, "RiverCrossing")]
        [TestCase(TableNameConstants.RoomExits, "RoomExits")]
        [TestCase(TableNameConstants.Rooms, "Rooms")]
        [TestCase(TableNameConstants.SidePassages, "SidePassages")]
        [TestCase(TableNameConstants.SpecialAREA, "Special{0}")]
        [TestCase(TableNameConstants.SpecialAreaShapes, "SpecialAreaShapes")]
        [TestCase(TableNameConstants.SpecialAreaSizes, "SpecialAreaSizes")]
        [TestCase(TableNameConstants.Stairs, "Stairs")]
        [TestCase(TableNameConstants.StreamCrossing, "StreamCrossing")]
        [TestCase(TableNameConstants.LowLevelTraps, "LowLevelTraps")]
        [TestCase(TableNameConstants.MidLevelTraps, "MidLevelTraps")]
        [TestCase(TableNameConstants.HighLevelTraps, "HighLevelTraps")]
        [TestCase(TableNameConstants.TreasureConcealment, "TreasureConcealment")]
        [TestCase(TableNameConstants.TreasureContainers, "TreasureContainers")]
        [TestCase(TableNameConstants.Turns, "Turns")]
        public void Constant(string constant, string value)
        {
            Assert.That(constant, Is.EqualTo(value));
        }
    }
}

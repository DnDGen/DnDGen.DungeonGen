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
        [TestCase(TableNameConstants.DoorLocations, "DoorLocations")]
        [TestCase(TableNameConstants.DoorType, "DoorType")]
        [TestCase(TableNameConstants.DungeonAreaFromHall, "DungeonAreaFromHall")]
        [TestCase(TableNameConstants.DungeonAreaFromDoor, "DungeonAreaFromDoor")]
        [TestCase(TableNameConstants.ExitDirection, "ExitDirection")]
        [TestCase(TableNameConstants.ExitLocation, "ExitLocation")]
        [TestCase(TableNameConstants.GalleryStairs, "GalleryStairs")]
        [TestCase(TableNameConstants.Halls, "Halls")]
        [TestCase(TableNameConstants.RiverCrossing, "RiverCrossing")]
        [TestCase(TableNameConstants.RoomExits, "RoomExits")]
        [TestCase(TableNameConstants.Rooms, "Rooms")]
        [TestCase(TableNameConstants.SidePassages, "SidePassages")]
        [TestCase(TableNameConstants.SpecialAREA, "Special{0}")]
        [TestCase(TableNameConstants.StreamCrossing, "StreamCrossing")]
        [TestCase(TableNameConstants.Turns, "Turns")]
        public void Constant(string constant, string value)
        {
            Assert.That(constant, Is.EqualTo(value));
        }
    }
}

using DungeonGen.Common;
using DungeonGen.Generators;
using DungeonGen.Generators.Domain.AreaGenerators;
using DungeonGen.Selectors;
using DungeonGen.Tables;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace DungeonGen.Tests.Unit.Generators.AreaGenerators
{
    [TestFixture]
    public class RoomGeneratorTests
    {
        private AreaGenerator roomGenerator;
        private Mock<IAreaPercentileSelector> mockAreaPercentileSelector;
        private Area selectedRoom;
        private Mock<AreaGenerator> mockSpecialChamberGenerator;
        private Mock<ExitGenerator> mockExitGenerator;
        private Mock<ContentsGenerator> mockContentsGenerator;

        [SetUp]
        public void Setup()
        {
            mockAreaPercentileSelector = new Mock<IAreaPercentileSelector>();
            mockSpecialChamberGenerator = new Mock<AreaGenerator>();
            mockExitGenerator = new Mock<ExitGenerator>();
            mockContentsGenerator = new Mock<ContentsGenerator>();
            roomGenerator = new RoomGenerator(mockAreaPercentileSelector.Object, mockSpecialChamberGenerator.Object, mockExitGenerator.Object, mockContentsGenerator.Object);

            selectedRoom = new Area();
            selectedRoom.Length = 9266;
            selectedRoom.Width = 90210;

            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.Rooms)).Returns(selectedRoom);
        }

        [Test]
        public void GenerateRoom()
        {
            var rooms = roomGenerator.Generate(42);
            Assert.That(rooms, Is.Not.Null);
            Assert.That(rooms, Is.Not.Empty);
        }

        [Test]
        public void GenerateRoomFromSelector()
        {
            var rooms = roomGenerator.Generate(42);
            Assert.That(rooms.Single(), Is.EqualTo(selectedRoom));
        }

        [Test]
        public void GenerateRoomExits()
        {
            var firstExit = new Area();
            var secondExit = new Area();
            mockExitGenerator.Setup(g => g.Generate(42, 9266, 90210)).Returns(new[] { firstExit, secondExit });

            var rooms = roomGenerator.Generate(42);
            Assert.That(rooms, Contains.Item(selectedRoom));
            Assert.That(rooms, Contains.Item(firstExit));
            Assert.That(rooms, Contains.Item(secondExit));
        }

        [Test]
        public void GenerateRoomContents()
        {
            var contents = new Contents();
            mockContentsGenerator.Setup(g => g.Generate(42)).Returns(contents);

            var rooms = roomGenerator.Generate(42);
            Assert.That(rooms.Single().Contents, Is.EqualTo(contents));
        }

        [Test]
        public void GenerateSpecialRoom()
        {
            selectedRoom.Type = AreaTypeConstants.Special;
            var firstSpecialArea = new Area();
            var secondSpecialArea = new Area();
            var specialAreas = new[] { firstSpecialArea, secondSpecialArea };

            mockSpecialChamberGenerator.Setup(g => g.Generate(42)).Returns(specialAreas);

            var rooms = roomGenerator.Generate(42);
            Assert.That(rooms, Is.EqualTo(specialAreas));
        }
    }
}

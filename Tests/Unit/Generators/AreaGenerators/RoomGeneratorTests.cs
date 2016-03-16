using DungeonGen.Common;
using DungeonGen.Generators;
using DungeonGen.Generators.Domain.AreaGenerators;
using DungeonGen.Selectors;
using DungeonGen.Tables;
using EncounterGen.Common;
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
        private Mock<AreaGenerator> mockSpecialAreaGenerator;
        private Mock<ExitGenerator> mockExitGenerator;
        private Mock<ContentsGenerator> mockContentsGenerator;

        [SetUp]
        public void Setup()
        {
            mockAreaPercentileSelector = new Mock<IAreaPercentileSelector>();
            mockSpecialAreaGenerator = new Mock<AreaGenerator>();
            mockExitGenerator = new Mock<ExitGenerator>();
            mockContentsGenerator = new Mock<ContentsGenerator>();
            roomGenerator = new RoomGenerator(mockAreaPercentileSelector.Object, mockSpecialAreaGenerator.Object, mockExitGenerator.Object, mockContentsGenerator.Object);

            selectedRoom = new Area();
            selectedRoom.Length = 9266;
            selectedRoom.Width = 90210;

            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.Rooms)).Returns(selectedRoom);
            mockContentsGenerator.Setup(g => g.Generate(It.IsAny<int>())).Returns(() => new Contents());
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
            var generatedContents = new Contents();
            generatedContents.Encounters = new[] { new Encounter(), new Encounter() };
            generatedContents.Miscellaneous = new[] { "thing 1", "thing 2" };
            generatedContents.Traps = new[] { new Trap(), new Trap() };
            generatedContents.Treasures = new[] { new ContainedTreasure(), new ContainedTreasure() };

            mockContentsGenerator.Setup(g => g.Generate(42)).Returns(generatedContents);

            var rooms = roomGenerator.Generate(42);
            var contents = rooms.Single().Contents;

            Assert.That(contents.Encounters.Count(), Is.EqualTo(2));
            Assert.That(contents.Miscellaneous, Contains.Item("thing 1"));
            Assert.That(contents.Miscellaneous, Contains.Item("thing 2"));
            Assert.That(contents.Miscellaneous.Count(), Is.EqualTo(2));
            Assert.That(contents.Traps.Count(), Is.EqualTo(2));
            Assert.That(contents.Treasures.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GenerateSpecialRoom()
        {
            selectedRoom.Type = AreaTypeConstants.Special;
            var firstSpecialArea = new Area();
            var secondSpecialArea = new Area();
            var specialAreas = new[] { firstSpecialArea, secondSpecialArea };

            mockSpecialAreaGenerator.Setup(g => g.Generate(42)).Returns(specialAreas);

            var rooms = roomGenerator.Generate(42);
            Assert.That(rooms, Is.EqualTo(specialAreas));
        }

        [Test]
        public void GenerateSpecialRoomWithContents()
        {
            selectedRoom.Type = AreaTypeConstants.Special;
            var firstSpecialArea = new Area();
            firstSpecialArea.Contents.Encounters = new[] { new Encounter() };
            firstSpecialArea.Contents.Traps = new[] { new Trap() };

            var secondSpecialArea = new Area();
            secondSpecialArea.Contents.Miscellaneous = new[] { "thing 1", "thing 2" };
            secondSpecialArea.Descriptions = new[] { "a cave" };
            secondSpecialArea.Contents.Treasures = new[] { new ContainedTreasure() };

            var firstContents = new Contents();
            firstContents.Miscellaneous = new[] { "new stuff" };
            firstContents.Encounters = new[] { new Encounter() };

            var secondContents = new Contents();
            secondContents.Miscellaneous = new[] { "other new stuff" };
            secondContents.Treasures = new[] { new ContainedTreasure() };
            secondContents.Traps = new[] { new Trap() };

            //INFO: We do the order backwards, becuase the internals will iterate backwards through the list
            mockContentsGenerator.SetupSequence(g => g.Generate(42)).Returns(secondContents).Returns(firstContents);

            var specialAreas = new[] { firstSpecialArea, secondSpecialArea };

            mockSpecialAreaGenerator.Setup(g => g.Generate(42)).Returns(specialAreas);

            var rooms = roomGenerator.Generate(42);
            Assert.That(rooms, Is.EqualTo(specialAreas));

            var first = rooms.First();
            var last = rooms.Last();

            Assert.That(first.Contents.Encounters.Count(), Is.EqualTo(2));
            Assert.That(first.Contents.Miscellaneous.Single(), Is.EqualTo("new stuff"));
            Assert.That(first.Contents.Traps.Count(), Is.EqualTo(1));
            Assert.That(first.Contents.Treasures, Is.Empty);
            Assert.That(first.Descriptions, Is.Empty);

            Assert.That(last.Contents.Encounters, Is.Empty);
            Assert.That(last.Contents.Miscellaneous, Contains.Item("thing 1"));
            Assert.That(last.Contents.Miscellaneous, Contains.Item("thing 2"));
            Assert.That(last.Contents.Miscellaneous, Contains.Item("other new stuff"));
            Assert.That(last.Contents.Miscellaneous.Count(), Is.EqualTo(3));
            Assert.That(last.Contents.Traps.Count(), Is.EqualTo(1));
            Assert.That(last.Contents.Treasures.Count(), Is.EqualTo(2));
            Assert.That(last.Descriptions.Single(), Is.EqualTo("a cave"));
        }

        [Test]
        public void GenerateSpecialRoomExits()
        {
            selectedRoom.Type = AreaTypeConstants.Special;

            var firstSpecialArea = new Area();
            firstSpecialArea.Length = 9266;
            firstSpecialArea.Width = 90210;

            var secondSpecialArea = new Area();
            secondSpecialArea.Length = 600;
            secondSpecialArea.Width = 1337;

            var specialAreas = new[] { firstSpecialArea, secondSpecialArea };

            mockSpecialAreaGenerator.Setup(g => g.Generate(42)).Returns(specialAreas);

            var firstExit = new Area();
            var secondExit = new Area();
            mockExitGenerator.Setup(g => g.Generate(42, 9266, 90210)).Returns(new[] { firstExit, secondExit });

            var thirdExit = new Area();
            var fourthExit = new Area();
            mockExitGenerator.Setup(g => g.Generate(42, 600, 1337)).Returns(new[] { thirdExit, fourthExit });

            var rooms = roomGenerator.Generate(42).ToArray();
            Assert.That(rooms[0], Is.EqualTo(firstSpecialArea));
            Assert.That(rooms[1], Is.EqualTo(firstExit));
            Assert.That(rooms[2], Is.EqualTo(secondExit));
            Assert.That(rooms[3], Is.EqualTo(secondSpecialArea));
            Assert.That(rooms[4], Is.EqualTo(thirdExit));
            Assert.That(rooms[5], Is.EqualTo(fourthExit));
        }
    }
}

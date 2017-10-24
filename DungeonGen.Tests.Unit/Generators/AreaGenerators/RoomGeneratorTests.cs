using DnDGen.Core.Generators;
using DungeonGen.Domain.Generators.AreaGenerators;
using DungeonGen.Domain.Generators.ContentGenerators;
using DungeonGen.Domain.Generators.ExitGenerators;
using DungeonGen.Domain.Generators.Factories;
using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using EncounterGen.Common;
using EncounterGen.Generators;
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
        private Mock<AreaGeneratorFactory> mockAreaGeneratorFactory;
        private Mock<JustInTimeFactory> mockJustInTimeFactory;
        private EncounterSpecifications specifications;

        [SetUp]
        public void Setup()
        {
            mockAreaPercentileSelector = new Mock<IAreaPercentileSelector>();
            mockSpecialAreaGenerator = new Mock<AreaGenerator>();
            mockExitGenerator = new Mock<ExitGenerator>();
            mockContentsGenerator = new Mock<ContentsGenerator>();
            mockAreaGeneratorFactory = new Mock<AreaGeneratorFactory>();
            mockJustInTimeFactory = new Mock<JustInTimeFactory>();
            roomGenerator = new RoomGenerator(mockAreaPercentileSelector.Object, mockAreaGeneratorFactory.Object, mockJustInTimeFactory.Object, mockContentsGenerator.Object);

            specifications = new EncounterSpecifications();
            selectedRoom = new Area();
            selectedRoom.Length = 9266;
            selectedRoom.Width = 90210;

            mockAreaGeneratorFactory.Setup(f => f.Build(AreaTypeConstants.Special)).Returns(mockSpecialAreaGenerator.Object);
            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.Rooms)).Returns(selectedRoom);
            mockContentsGenerator.Setup(g => g.Generate(It.IsAny<int>())).Returns(() => new Contents());
            mockJustInTimeFactory.Setup(f => f.Build<ExitGenerator>(AreaTypeConstants.Room)).Returns(mockExitGenerator.Object);
        }

        [Test]
        public void AreaTypeIsRoom()
        {
            Assert.That(roomGenerator.AreaType, Is.EqualTo(AreaTypeConstants.Room));
        }

        [Test]
        public void GenerateRoom()
        {
            var rooms = roomGenerator.Generate(42, specifications);
            Assert.That(rooms, Is.Not.Null);
            Assert.That(rooms, Is.Not.Empty);
        }

        [Test]
        public void GenerateRoomFromSelector()
        {
            var rooms = roomGenerator.Generate(42, specifications);
            Assert.That(rooms.Single(), Is.EqualTo(selectedRoom));
        }

        [Test]
        public void GenerateRoomExits()
        {
            var firstExit = new Area();
            var secondExit = new Area();
            mockExitGenerator.Setup(g => g.Generate(42, specifications, 9266, 90210)).Returns(new[] { firstExit, secondExit });

            var rooms = roomGenerator.Generate(42, specifications);
            Assert.That(rooms, Contains.Item(selectedRoom));
            Assert.That(rooms, Contains.Item(firstExit));
            Assert.That(rooms, Contains.Item(secondExit));
        }

        [Test]
        public void GenerateRoomContents()
        {
            specifications.Level = 600;

            var generatedContents = new Contents();
            generatedContents.Encounters = new[] { new Encounter(), new Encounter() };
            generatedContents.Miscellaneous = new[] { "thing 1", "thing 2" };
            generatedContents.Traps = new[] { new Trap(), new Trap() };
            generatedContents.Treasures = new[] { new DungeonTreasure(), new DungeonTreasure() };

            mockContentsGenerator.Setup(g => g.Generate(600)).Returns(generatedContents);

            var rooms = roomGenerator.Generate(42, specifications);
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

            mockSpecialAreaGenerator.Setup(g => g.Generate(42, specifications)).Returns(specialAreas);

            var rooms = roomGenerator.Generate(42, specifications);
            Assert.That(rooms, Is.EqualTo(specialAreas));
        }

        [Test]
        public void GenerateSpecialRoomWithContents()
        {
            specifications.Level = 600;

            selectedRoom.Type = AreaTypeConstants.Special;
            var firstSpecialArea = new Area();
            firstSpecialArea.Contents.Encounters = new[] { new Encounter() };
            firstSpecialArea.Contents.Traps = new[] { new Trap() };

            var secondSpecialArea = new Area();
            secondSpecialArea.Contents.Miscellaneous = new[] { "thing 1", "thing 2" };
            secondSpecialArea.Descriptions = new[] { "a cave" };
            secondSpecialArea.Contents.Treasures = new[] { new DungeonTreasure() };

            var firstContents = new Contents();
            firstContents.Miscellaneous = new[] { "new stuff" };
            firstContents.Encounters = new[] { new Encounter() };

            var secondContents = new Contents();
            secondContents.Miscellaneous = new[] { "other new stuff" };
            secondContents.Treasures = new[] { new DungeonTreasure() };
            secondContents.Traps = new[] { new Trap() };

            //INFO: We do the order backwards, becuase the internals will iterate backwards through the list
            mockContentsGenerator.SetupSequence(g => g.Generate(600)).Returns(secondContents).Returns(firstContents);

            var specialAreas = new[] { firstSpecialArea, secondSpecialArea };

            mockSpecialAreaGenerator.Setup(g => g.Generate(42, specifications)).Returns(specialAreas);

            var rooms = roomGenerator.Generate(42, specifications);
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
            secondSpecialArea.Length = 1234;
            secondSpecialArea.Width = 1337;

            var specialAreas = new[] { firstSpecialArea, secondSpecialArea };

            mockSpecialAreaGenerator.Setup(g => g.Generate(42, specifications)).Returns(specialAreas);

            var firstExit = new Area();
            var secondExit = new Area();
            mockExitGenerator.Setup(g => g.Generate(42, specifications, 9266, 90210)).Returns(new[] { firstExit, secondExit });

            var thirdExit = new Area();
            var fourthExit = new Area();
            mockExitGenerator.Setup(g => g.Generate(42, specifications, 1234, 1337)).Returns(new[] { thirdExit, fourthExit });

            var rooms = roomGenerator.Generate(42, specifications).ToArray();
            Assert.That(rooms[0], Is.EqualTo(firstSpecialArea));
            Assert.That(rooms[1], Is.EqualTo(firstExit));
            Assert.That(rooms[2], Is.EqualTo(secondExit));
            Assert.That(rooms[3], Is.EqualTo(secondSpecialArea));
            Assert.That(rooms[4], Is.EqualTo(thirdExit));
            Assert.That(rooms[5], Is.EqualTo(fourthExit));
        }

        [Test]
        public void IfSpecialAreaTypeIsBlank_AssignRoom()
        {
            selectedRoom.Type = AreaTypeConstants.Special;
            var firstSpecialArea = new Area();
            var secondSpecialArea = new Area();
            var specialAreas = new[] { firstSpecialArea, secondSpecialArea };

            mockSpecialAreaGenerator.Setup(g => g.Generate(42, specifications)).Returns(specialAreas);

            var rooms = roomGenerator.Generate(42, specifications);
            Assert.That(rooms, Is.EqualTo(specialAreas));
            Assert.That(rooms.First().Type, Is.EqualTo(AreaTypeConstants.Room));
            Assert.That(rooms.Last().Type, Is.EqualTo(AreaTypeConstants.Room));
        }

        [Test]
        public void IfSpecialAreaTypeIsNotBlank_DoNotAssignRoom()
        {
            selectedRoom.Type = AreaTypeConstants.Special;
            var firstSpecialArea = new Area { Type = "cave" };
            var secondSpecialArea = new Area { Type = "whatever" };
            var specialAreas = new[] { firstSpecialArea, secondSpecialArea };

            mockSpecialAreaGenerator.Setup(g => g.Generate(42, specifications)).Returns(specialAreas);

            var rooms = roomGenerator.Generate(42, specifications);
            Assert.That(rooms, Is.EqualTo(specialAreas));
            Assert.That(rooms.First().Type, Is.EqualTo("cave"));
            Assert.That(rooms.Last().Type, Is.EqualTo("whatever"));
        }

        [Test]
        public void DetermineWhetherToAssignRoomPerSpecialArea()
        {
            selectedRoom.Type = AreaTypeConstants.Special;
            var firstSpecialArea = new Area { Type = "cave" };
            var secondSpecialArea = new Area();
            var specialAreas = new[] { firstSpecialArea, secondSpecialArea };

            mockSpecialAreaGenerator.Setup(g => g.Generate(42, specifications)).Returns(specialAreas);

            var rooms = roomGenerator.Generate(42, specifications);
            Assert.That(rooms, Is.EqualTo(specialAreas));
            Assert.That(rooms.First().Type, Is.EqualTo("cave"));
            Assert.That(rooms.Last().Type, Is.EqualTo(AreaTypeConstants.Room));
        }
    }
}

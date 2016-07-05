using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.ExitGenerators;
using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace DungeonGen.Tests.Unit.Generators.AreaGenerators
{
    [TestFixture]
    public class RoomExitGeneratorTests
    {
        private ExitGenerator roomExitGenerator;
        private Mock<IAreaPercentileSelector> mockAreaPercentileSelector;
        private Mock<AreaGenerator> mockHallGenerator;
        private Mock<AreaGenerator> mockDoorGenerator;
        private Area selectedExit;
        private Mock<IPercentileSelector> mockPercentileSelector;

        [SetUp]
        public void Setup()
        {
            mockAreaPercentileSelector = new Mock<IAreaPercentileSelector>();
            mockHallGenerator = new Mock<AreaGenerator>();
            mockDoorGenerator = new Mock<AreaGenerator>();
            mockPercentileSelector = new Mock<IPercentileSelector>();
            roomExitGenerator = new RoomExitGenerator(mockAreaPercentileSelector.Object, mockHallGenerator.Object, mockDoorGenerator.Object, mockPercentileSelector.Object);

            selectedExit = new Area();
            selectedExit.Type = "exit type";
            selectedExit.Width = 1;
            selectedExit.Length = 42 * 600;

            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.RoomExits)).Returns(selectedExit);
        }

        [Test]
        public void GetExitFromSelector()
        {
            var exit = new Area();
            mockDoorGenerator.Setup(g => g.Generate(9266, 90210)).Returns(new[] { exit });

            var exits = roomExitGenerator.Generate(9266, 90210, 42, 600);
            Assert.That(exits.Single(), Is.EqualTo(exit));
        }

        [Test]
        public void GetMultipleExits()
        {
            selectedExit.Width = 2;

            var exit = new Area();
            var otherExit = new Area();
            mockDoorGenerator.SetupSequence(g => g.Generate(9266, 90210)).Returns(new[] { exit }).Returns(new[] { otherExit });

            var exits = roomExitGenerator.Generate(9266, 90210, 42, 600);
            Assert.That(exits.Count(), Is.EqualTo(2));

            var first = exits.First();
            var last = exits.Last();

            Assert.That(first, Is.EqualTo(exit));
            Assert.That(last, Is.EqualTo(otherExit));
        }

        [Test]
        public void GetNoExits()
        {
            selectedExit.Width = 0;

            var exits = roomExitGenerator.Generate(9266, 90210, 42, 600);
            Assert.That(exits, Is.Empty);
        }

        [TestCase(0, 1)]
        [TestCase(1, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 4)]
        public void IfAreaBiggerThanLimit_AddExtraExit(int originalExits, int totalExits)
        {
            selectedExit.Length -= 1;
            selectedExit.Width = originalExits;
            mockDoorGenerator.Setup(g => g.Generate(9266, 90210)).Returns(() => new[] { new Area() });

            var exits = roomExitGenerator.Generate(9266, 90210, 42, 600);
            Assert.That(exits.Count(), Is.EqualTo(totalExits));
        }

        [Test]
        public void IfLimitIsZero_DoNotGetExtraExit()
        {
            selectedExit.Length = 0;
            mockDoorGenerator.Setup(g => g.Generate(9266, 90210)).Returns(() => new[] { new Area() });

            var exits = roomExitGenerator.Generate(9266, 90210, 42, 600);
            Assert.That(exits.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GetHall()
        {
            selectedExit.Type = AreaTypeConstants.Hall;

            var hall = new Area { Type = AreaTypeConstants.Hall };
            mockHallGenerator.Setup(g => g.Generate(9266, 90210)).Returns(new[] { hall });

            var exits = roomExitGenerator.Generate(9266, 90210, 42, 600);
            Assert.That(exits.Single(), Is.EqualTo(hall));
        }

        [Test]
        public void GetExitLocationAndDirectionForHall()
        {
            selectedExit.Type = AreaTypeConstants.Hall;

            var hall = new Area { Type = AreaTypeConstants.Hall };
            mockHallGenerator.Setup(g => g.Generate(9266, 90210)).Returns(new[] { hall });
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.ExitLocation)).Returns("on the ceiling");
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.ExitDirection)).Returns("to the right");

            var exits = roomExitGenerator.Generate(9266, 90210, 42, 600);
            Assert.That(exits.Single(), Is.EqualTo(hall));
            Assert.That(hall.Descriptions, Contains.Item("on the ceiling"));
            Assert.That(hall.Descriptions, Contains.Item("to the right"));
            Assert.That(hall.Descriptions.Count(), Is.EqualTo(2));
        }

        [Test]
        public void ExitLocationAndDirectionDoNotOverwriteHallDescriptions()
        {
            selectedExit.Type = AreaTypeConstants.Hall;

            var hall = new Area { Type = AreaTypeConstants.Hall, Descriptions = new[] { "dark", "dank" } };
            mockHallGenerator.Setup(g => g.Generate(9266, 90210)).Returns(new[] { hall });
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.ExitLocation)).Returns("on the ceiling");
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.ExitDirection)).Returns("to the right");

            var exits = roomExitGenerator.Generate(9266, 90210, 42, 600);
            Assert.That(exits.Single(), Is.EqualTo(hall));
            Assert.That(hall.Descriptions, Contains.Item("on the ceiling"));
            Assert.That(hall.Descriptions, Contains.Item("to the right"));
            Assert.That(hall.Descriptions, Contains.Item("dark"));
            Assert.That(hall.Descriptions, Contains.Item("dank"));
            Assert.That(hall.Descriptions.Count(), Is.EqualTo(4));
        }

        [Test]
        public void GetExitLocations()
        {
            selectedExit.Width = 2;

            var exit = new Area { Type = AreaTypeConstants.Door };
            var otherExit = new Area { Type = AreaTypeConstants.Door };
            mockDoorGenerator.SetupSequence(g => g.Generate(9266, 90210)).Returns(new[] { exit }).Returns(new[] { otherExit });

            mockPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.ExitLocation)).Returns("on the ceiling").Returns("behind you");
            mockPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.ExitDirection)).Returns("to the right").Returns("to the left");

            var exits = roomExitGenerator.Generate(9266, 90210, 42, 600);
            Assert.That(exits.Count(), Is.EqualTo(2));

            var first = exits.First();
            var last = exits.Last();

            Assert.That(first, Is.EqualTo(exit));
            Assert.That(first.Descriptions.Single(), Is.EqualTo("on the ceiling"));
            Assert.That(last, Is.EqualTo(otherExit));
            Assert.That(last.Descriptions.Single(), Is.EqualTo("behind you"));
        }

        [Test]
        public void ExitLocationsDoNotOverwriteDoorDescriptions()
        {
            selectedExit.Width = 2;

            var exit = new Area { Type = AreaTypeConstants.Door, Descriptions = new[] { "door-like" } };
            var otherExit = new Area { Type = AreaTypeConstants.Door, Descriptions = new[] { "sliding", "like Star Trek" } };
            mockDoorGenerator.SetupSequence(g => g.Generate(9266, 90210)).Returns(new[] { exit }).Returns(new[] { otherExit });

            mockPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.ExitLocation)).Returns("on the ceiling").Returns("behind you");
            mockPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.ExitDirection)).Returns("to the right").Returns("to the left");

            var exits = roomExitGenerator.Generate(9266, 90210, 42, 600);
            Assert.That(exits.Count(), Is.EqualTo(2));

            var first = exits.First();
            var last = exits.Last();

            Assert.That(first, Is.EqualTo(exit));
            Assert.That(first.Descriptions, Contains.Item("on the ceiling"));
            Assert.That(first.Descriptions, Contains.Item("door-like"));
            Assert.That(first.Descriptions.Count(), Is.EqualTo(2));
            Assert.That(last, Is.EqualTo(otherExit));
            Assert.That(last.Descriptions, Contains.Item("behind you"));
            Assert.That(last.Descriptions, Contains.Item("sliding"));
            Assert.That(last.Descriptions, Contains.Item("like Star Trek"));
            Assert.That(last.Descriptions.Count(), Is.EqualTo(3));
        }
    }
}

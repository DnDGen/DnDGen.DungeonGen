using DungeonGen.Domain.Generators.AreaGenerators;
using DungeonGen.Domain.Generators.ExitGenerators;
using DungeonGen.Domain.Generators.Factories;
using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace DungeonGen.Tests.Unit.Generators.ExitGenerators
{
    [TestFixture]
    public class ChamberExitGeneratorTests
    {
        private ExitGenerator chamberExitGenerator;
        private Mock<IAreaPercentileSelector> mockAreaPercentileSelector;
        private Mock<AreaGenerator> mockHallGenerator;
        private Mock<AreaGenerator> mockDoorGenerator;
        private Area selectedExit;
        private Mock<IPercentileSelector> mockPercentileSelector;
        private Mock<AreaGeneratorFactory> mockAreaGeneratorFactory;

        [SetUp]
        public void Setup()
        {
            mockAreaPercentileSelector = new Mock<IAreaPercentileSelector>();
            mockHallGenerator = new Mock<AreaGenerator>();
            mockDoorGenerator = new Mock<AreaGenerator>();
            mockPercentileSelector = new Mock<IPercentileSelector>();
            mockAreaGeneratorFactory = new Mock<AreaGeneratorFactory>();
            chamberExitGenerator = new ChamberExitGenerator(mockAreaPercentileSelector.Object, mockAreaGeneratorFactory.Object, mockPercentileSelector.Object);

            selectedExit = new Area();
            selectedExit.Type = "exit type";
            selectedExit.Width = 1;
            selectedExit.Length = 42 * 600;
            selectedExit.Type = AreaTypeConstants.Hall;

            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.ChamberExits)).Returns(selectedExit);
            mockAreaGeneratorFactory.Setup(f => f.Build(AreaTypeConstants.Hall)).Returns(mockHallGenerator.Object);
            mockAreaGeneratorFactory.Setup(f => f.Build(AreaTypeConstants.Door)).Returns(mockDoorGenerator.Object);
        }

        [Test]
        public void GetExitFromSelector()
        {
            var exit = new Area();
            mockHallGenerator.Setup(g => g.Generate(9266, 90210, "temperature")).Returns(new[] { exit });

            var exits = chamberExitGenerator.Generate(9266, 90210, 42, 600, "temperature");
            Assert.That(exits.Single(), Is.EqualTo(exit));
        }

        [Test]
        public void GetMultipleExits()
        {
            selectedExit.Width = 2;

            var exit = new Area();
            var otherExit = new Area();
            mockHallGenerator.SetupSequence(g => g.Generate(9266, 90210, "temperature")).Returns(new[] { exit }).Returns(new[] { otherExit });

            var exits = chamberExitGenerator.Generate(9266, 90210, 42, 600, "temperature");
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

            var exits = chamberExitGenerator.Generate(9266, 90210, 42, 600, "temperature");
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

            mockHallGenerator.Setup(g => g.Generate(9266, 90210, "temperature")).Returns(() => new[] { new Area() });

            var exits = chamberExitGenerator.Generate(9266, 90210, 42, 600, "temperature");
            Assert.That(exits.Count(), Is.EqualTo(totalExits));
        }

        [Test]
        public void IfLimitIsZero_DoNotGetExtraExit()
        {
            selectedExit.Length = 0;

            mockHallGenerator.Setup(g => g.Generate(9266, 90210, "temperature")).Returns(() => new[] { new Area() });

            var exits = chamberExitGenerator.Generate(9266, 90210, 42, 600, "temperature");
            Assert.That(exits.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GetDoor()
        {
            selectedExit.Type = AreaTypeConstants.Door;

            var door = new Area { Type = AreaTypeConstants.Door };
            mockDoorGenerator.Setup(g => g.Generate(9266, 90210, "temperature")).Returns(new[] { door });

            var exits = chamberExitGenerator.Generate(9266, 90210, 42, 600, "temperature");
            Assert.That(exits.Single(), Is.EqualTo(door));
        }

        [Test]
        public void GetOnlyExitLocationForDoor()
        {
            selectedExit.Type = AreaTypeConstants.Door;

            var door = new Area { Type = AreaTypeConstants.Door };
            mockDoorGenerator.Setup(g => g.Generate(9266, 90210, "temperature")).Returns(new[] { door });
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.ExitLocation)).Returns("on the ceiling");
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.ExitDirection)).Returns("to the right");

            var exits = chamberExitGenerator.Generate(9266, 90210, 42, 600, "temperature");
            Assert.That(exits.Single(), Is.EqualTo(door));
            Assert.That(door.Descriptions.Single(), Is.EqualTo("on the ceiling"));
        }

        [Test]
        public void ExitLocationDoesNotOverewriteDoorDescriptions()
        {
            selectedExit.Type = AreaTypeConstants.Door;

            var door = new Area { Type = AreaTypeConstants.Door, Descriptions = new[] { "secret", "speak friend and enter" } };
            mockDoorGenerator.Setup(g => g.Generate(9266, 90210, "temperature")).Returns(new[] { door });
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.ExitLocation)).Returns("on the ceiling");
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.ExitDirection)).Returns("to the right");

            var exits = chamberExitGenerator.Generate(9266, 90210, 42, 600, "temperature");
            Assert.That(exits.Single(), Is.EqualTo(door));
            Assert.That(door.Descriptions, Contains.Item("on the ceiling"));
            Assert.That(door.Descriptions, Contains.Item("secret"));
            Assert.That(door.Descriptions, Contains.Item("speak friend and enter"));
            Assert.That(door.Descriptions.Count(), Is.EqualTo(3));
        }

        [Test]
        public void GetExitLocationsAndDirections()
        {
            selectedExit.Width = 2;

            var exit = new Area();
            var otherExit = new Area();
            mockHallGenerator.SetupSequence(g => g.Generate(9266, 90210, "temperature")).Returns(new[] { exit }).Returns(new[] { otherExit });

            mockPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.ExitLocation)).Returns("on the ceiling").Returns("behind you");
            mockPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.ExitDirection)).Returns("to the right").Returns("to the left");

            var exits = chamberExitGenerator.Generate(9266, 90210, 42, 600, "temperature");
            Assert.That(exits.Count(), Is.EqualTo(2));

            var first = exits.First();
            var last = exits.Last();

            Assert.That(first, Is.EqualTo(exit));
            Assert.That(first.Descriptions, Contains.Item("on the ceiling"));
            Assert.That(first.Descriptions, Contains.Item("to the right"));
            Assert.That(first.Descriptions.Count(), Is.EqualTo(2));
            Assert.That(last, Is.EqualTo(otherExit));
            Assert.That(last.Descriptions, Contains.Item("behind you"));
            Assert.That(last.Descriptions, Contains.Item("to the left"));
            Assert.That(last.Descriptions.Count(), Is.EqualTo(2));
        }

        [Test]
        public void ExitLocationsAndDirectionsDoNotOverwriteHallDescriptions()
        {
            selectedExit.Width = 2;

            var exit = new Area { Descriptions = new[] { "hallway-y" } };
            var otherExit = new Area { Descriptions = new[] { "dark", "dank" } };
            mockHallGenerator.SetupSequence(g => g.Generate(9266, 90210, "temperature")).Returns(new[] { exit }).Returns(new[] { otherExit });

            mockPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.ExitLocation)).Returns("on the ceiling").Returns("behind you");
            mockPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.ExitDirection)).Returns("to the right").Returns("to the left");

            var exits = chamberExitGenerator.Generate(9266, 90210, 42, 600, "temperature");
            Assert.That(exits.Count(), Is.EqualTo(2));

            var first = exits.First();
            var last = exits.Last();

            Assert.That(first, Is.EqualTo(exit));
            Assert.That(first.Descriptions, Contains.Item("on the ceiling"));
            Assert.That(first.Descriptions, Contains.Item("to the right"));
            Assert.That(first.Descriptions, Contains.Item("hallway-y"));
            Assert.That(first.Descriptions.Count(), Is.EqualTo(3));
            Assert.That(last, Is.EqualTo(otherExit));
            Assert.That(last.Descriptions, Contains.Item("behind you"));
            Assert.That(last.Descriptions, Contains.Item("to the left"));
            Assert.That(last.Descriptions, Contains.Item("dark"));
            Assert.That(last.Descriptions, Contains.Item("dank"));
            Assert.That(last.Descriptions.Count(), Is.EqualTo(4));
        }
    }
}

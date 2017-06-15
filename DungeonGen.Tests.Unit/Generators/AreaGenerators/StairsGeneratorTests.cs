using DungeonGen.Domain.Generators.AreaGenerators;
using DungeonGen.Domain.Generators.Factories;
using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using Moq;
using NUnit.Framework;
using RollGen;
using System.Linq;

namespace DungeonGen.Tests.Unit.Generators.AreaGenerators
{
    [TestFixture]
    public class StairsGeneratorTests
    {
        private AreaGenerator stairsGenerator;
        private Mock<IAreaPercentileSelector> mockAreaPercentileSelector;
        private Area selectedStairs;
        private Mock<Dice> mockDice;
        private Mock<AreaGenerator> mockChamberGenerator;
        private Mock<AreaGeneratorFactory> mockAreaGeneratorFactory;

        [SetUp]
        public void Setup()
        {
            mockAreaPercentileSelector = new Mock<IAreaPercentileSelector>();
            mockDice = new Mock<Dice>();
            mockChamberGenerator = new Mock<AreaGenerator>();
            mockAreaGeneratorFactory = new Mock<AreaGeneratorFactory>();
            stairsGenerator = new StairsGenerator(mockAreaPercentileSelector.Object, mockDice.Object, mockAreaGeneratorFactory.Object);
            selectedStairs = new Area();

            mockAreaGeneratorFactory.Setup(f => f.Build(AreaTypeConstants.Chamber)).Returns(mockChamberGenerator.Object);
            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.Stairs)).Returns(selectedStairs);
            mockDice.Setup(d => d.Roll(1).d(100).AsSum()).Returns(1);
        }

        [Test]
        public void AreaTypeIsStairs()
        {
            Assert.That(stairsGenerator.AreaType, Is.EqualTo(AreaTypeConstants.Stairs));
        }

        [Test]
        public void GenerateStairs()
        {
            var stairs = stairsGenerator.Generate(9266, 90210, "temperature");
            Assert.That(stairs, Is.Not.Null);
            Assert.That(stairs, Is.Not.Empty);
            Assert.That(stairs.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GenerateStairsDownXLevels()
        {
            selectedStairs.Length = 600;

            var stairs = stairsGenerator.Generate(9266, 90210, "temperature").Single();
            Assert.That(stairs, Is.EqualTo(selectedStairs));
            Assert.That(stairs.Descriptions.Single(), Is.EqualTo("Down to level 9866"));
            Assert.That(stairs.Length, Is.EqualTo(0));
            Assert.That(stairs.Width, Is.EqualTo(0));
        }

        [TestCase(41)]
        [TestCase(42)]
        public void GenerateStairsDownXLevelsDeadEnds(int roll)
        {
            selectedStairs.Length = 600;
            selectedStairs.Width = 42;

            mockDice.Setup(d => d.Roll(1).d(100).AsSum()).Returns(roll);

            var stairs = stairsGenerator.Generate(9266, 90210, "temperature");
            Assert.That(stairs.Count(), Is.EqualTo(2));

            var first = stairs.First();
            var last = stairs.Last();

            Assert.That(first, Is.EqualTo(selectedStairs));
            Assert.That(first.Descriptions.Single(), Is.EqualTo("Down to level 9866"));
            Assert.That(first.Length, Is.EqualTo(0));
            Assert.That(first.Width, Is.EqualTo(0));
            Assert.That(last.Type, Is.EqualTo(AreaTypeConstants.DeadEnd));
            Assert.That(last.Contents.IsEmpty, Is.True);
            Assert.That(last.Descriptions, Is.Empty);
            Assert.That(last.Length, Is.EqualTo(0));
            Assert.That(last.Width, Is.EqualTo(0));
        }

        [Test]
        public void GenerateStairsDownXLevelsDoesNotDeadEnd()
        {
            selectedStairs.Length = 600;
            selectedStairs.Width = 42;

            mockDice.Setup(d => d.Roll(1).d(100).AsSum()).Returns(43);

            var stairs = stairsGenerator.Generate(9266, 90210, "temperature").Single();
            Assert.That(stairs, Is.EqualTo(selectedStairs));
            Assert.That(stairs.Descriptions.Single(), Is.EqualTo("Down to level 9866"));
            Assert.That(stairs.Length, Is.EqualTo(0));
            Assert.That(stairs.Width, Is.EqualTo(0));
        }

        [Test]
        public void GenerateStairsUpXLevels()
        {
            selectedStairs.Length = -42;

            var stairs = stairsGenerator.Generate(9266, 90210, "temperature").Single();
            Assert.That(stairs, Is.EqualTo(selectedStairs));
            Assert.That(stairs.Descriptions.Single(), Is.EqualTo("Up to level 9224"));
            Assert.That(stairs.Length, Is.EqualTo(0));
            Assert.That(stairs.Width, Is.EqualTo(0));
        }

        [TestCase(-9266)]
        [TestCase(-9267)]
        public void IfUpTooManyLevels_ThenDeadEnd(int levelsDown)
        {
            selectedStairs.Length = levelsDown;

            var stairs = stairsGenerator.Generate(9266, 90210, "temperature").Single();
            Assert.That(stairs.Type, Is.EqualTo(AreaTypeConstants.DeadEnd));
            Assert.That(stairs.Contents.IsEmpty, Is.True);
            Assert.That(stairs.Descriptions, Is.Empty);
            Assert.That(stairs.Length, Is.EqualTo(0));
            Assert.That(stairs.Width, Is.EqualTo(0));
        }

        [Test]
        public void ChimneyAlsoLetsTheHallContinue()
        {
            selectedStairs.Length = -600;
            selectedStairs.Descriptions = new[] { DescriptionConstants.Chimney };

            var stairs = stairsGenerator.Generate(9266, 90210, "temperature");
            Assert.That(stairs.Count(), Is.EqualTo(2));

            var first = stairs.First();
            var last = stairs.Last();

            Assert.That(first.Type, Is.EqualTo(AreaTypeConstants.Hall));
            Assert.That(first.Contents.IsEmpty, Is.True);
            Assert.That(first.Descriptions, Is.Empty);
            Assert.That(first.Length, Is.EqualTo(30));
            Assert.That(first.Width, Is.EqualTo(0));
            Assert.That(last, Is.EqualTo(selectedStairs));
            Assert.That(last.Descriptions, Contains.Item("Up to level 8666"));
            Assert.That(last.Descriptions, Contains.Item(DescriptionConstants.Chimney));
            Assert.That(last.Descriptions.Count(), Is.EqualTo(2));
            Assert.That(last.Length, Is.EqualTo(0));
            Assert.That(last.Width, Is.EqualTo(0));
        }

        [TestCase(-9266)]
        [TestCase(-9267)]
        public void IfChimneyGoesUpTooHigh_NoChimney(int levelsDown)
        {
            selectedStairs.Length = levelsDown;
            selectedStairs.Descriptions = new[] { DescriptionConstants.Chimney };

            var stairs = stairsGenerator.Generate(9266, 90210, "temperature").Single();
            Assert.That(stairs.Type, Is.EqualTo(AreaTypeConstants.Hall));
            Assert.That(stairs.Contents.IsEmpty, Is.True);
            Assert.That(stairs.Descriptions, Is.Empty);
            Assert.That(stairs.Length, Is.EqualTo(30));
            Assert.That(stairs.Width, Is.EqualTo(0));
        }

        [Test]
        public void TrapDoorAlsoLetsTheHallContinue()
        {
            selectedStairs.Length = 600;
            selectedStairs.Descriptions = new[] { DescriptionConstants.TrapDoor };

            var stairs = stairsGenerator.Generate(9266, 90210, "temperature");
            Assert.That(stairs.Count(), Is.EqualTo(2));

            var first = stairs.First();
            var last = stairs.Last();

            Assert.That(first.Type, Is.EqualTo(AreaTypeConstants.Hall));
            Assert.That(first.Contents.IsEmpty, Is.True);
            Assert.That(first.Descriptions, Is.Empty);
            Assert.That(first.Length, Is.EqualTo(30));
            Assert.That(first.Width, Is.EqualTo(0));
            Assert.That(last, Is.EqualTo(selectedStairs));
            Assert.That(last.Descriptions, Contains.Item("Down to level 9866"));
            Assert.That(last.Descriptions, Contains.Item(DescriptionConstants.TrapDoor));
            Assert.That(last.Descriptions.Count(), Is.EqualTo(2));
            Assert.That(last.Length, Is.EqualTo(0));
            Assert.That(last.Width, Is.EqualTo(0));
        }

        [Test]
        public void StairsEndInChamber()
        {
            selectedStairs.Length = 600;
            selectedStairs.Contents.Miscellaneous = new[] { AreaTypeConstants.Chamber };

            var chamber = new Area();
            var exit = new Area();

            mockChamberGenerator.Setup(g => g.Generate(9266, 90210, "temperature")).Returns(new[] { chamber, exit });

            var stairs = stairsGenerator.Generate(9266, 90210, "temperature");
            Assert.That(stairs.Count(), Is.EqualTo(3));
            Assert.That(stairs, Contains.Item(selectedStairs));
            Assert.That(stairs, Contains.Item(chamber));
            Assert.That(stairs, Contains.Item(exit));

            var first = stairs.First();

            Assert.That(first, Is.EqualTo(selectedStairs));
            Assert.That(first.Descriptions.Single(), Is.EqualTo("Down to level 9866"));
            Assert.That(first.Contents.IsEmpty, Is.True);
            Assert.That(first.Length, Is.EqualTo(0));
            Assert.That(first.Width, Is.EqualTo(0));
        }
    }
}

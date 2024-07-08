using DnDGen.DungeonGen.Generators.AreaGenerators;
using DnDGen.DungeonGen.Generators.Factories;
using DnDGen.DungeonGen.Models;
using DnDGen.DungeonGen.Tables;
using DnDGen.EncounterGen.Generators;
using DnDGen.Infrastructure.Selectors.Percentiles;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace DnDGen.DungeonGen.Tests.Unit.Generators.AreaGenerators
{
    [TestFixture]
    public class SidePassageGeneratorTests
    {
        private AreaGenerator sidePassageGenerator;
        private Mock<AreaGenerator> mockHallGenerator;
        private Mock<IPercentileSelector> mockPercentileSelector;
        private Mock<AreaGeneratorFactory> mockAreaGeneratorFactory;
        private EncounterSpecifications specifications;

        [SetUp]
        public void Setup()
        {
            mockHallGenerator = new Mock<AreaGenerator>();
            mockPercentileSelector = new Mock<IPercentileSelector>();
            mockAreaGeneratorFactory = new Mock<AreaGeneratorFactory>();
            sidePassageGenerator = new SidePassageGenerator(mockPercentileSelector.Object, mockAreaGeneratorFactory.Object);

            specifications = new EncounterSpecifications();
            mockAreaGeneratorFactory.Setup(f => f.Build(AreaTypeConstants.Hall)).Returns(mockHallGenerator.Object);
        }

        [Test]
        public void AreaTypeIsSidePassage()
        {
            Assert.That(sidePassageGenerator.AreaType, Is.EqualTo(AreaTypeConstants.SidePassage));
        }

        [Test]
        public void GeneratorReturnsOriginalHallwayAndSidePassage()
        {
            mockPercentileSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.SidePassages)).Returns("description");

            var generatedSidePassage = new Area();
            mockHallGenerator.Setup(g => g.Generate(9266, specifications)).Returns(new[] { generatedSidePassage });

            var sidePassages = sidePassageGenerator.Generate(9266, specifications);
            Assert.That(sidePassages.Count(), Is.EqualTo(2));

            var originalHall = sidePassages.First();
            var sidePassage = sidePassages.Last();

            Assert.That(originalHall.Type, Is.EqualTo(AreaTypeConstants.Hall));
            Assert.That(originalHall.Length, Is.EqualTo(30));
            Assert.That(originalHall.Width, Is.EqualTo(0));

            Assert.That(sidePassage, Is.EqualTo(generatedSidePassage));
            Assert.That(sidePassage.Descriptions.Single(), Is.EqualTo("description"));
        }

        [Test]
        public void GenerateTIntersection()
        {
            mockPercentileSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.SidePassages)).Returns(SidePassageConstants.TIntersection);

            var leftHall = new Area();
            var rightHall = new Area();
            mockHallGenerator.SetupSequence(g => g.Generate(9266, specifications)).Returns(new[] { leftHall }).Returns(new[] { rightHall });

            var sidePassages = sidePassageGenerator.Generate(9266, specifications);
            Assert.That(sidePassages.Count(), Is.EqualTo(2));

            var first = sidePassages.First();
            var last = sidePassages.Last();

            Assert.That(first, Is.EqualTo(leftHall));
            Assert.That(first.Descriptions.Single(), Is.EqualTo(SidePassageConstants.Left90Degrees));
            Assert.That(last, Is.EqualTo(rightHall));
            Assert.That(last.Descriptions.Single(), Is.EqualTo(SidePassageConstants.Right90Degrees));
        }

        [Test]
        public void GenerateYIntersection()
        {
            mockPercentileSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.SidePassages)).Returns(SidePassageConstants.YIntersection);

            var leftHall = new Area();
            var rightHall = new Area();
            mockHallGenerator.SetupSequence(g => g.Generate(9266, specifications)).Returns(new[] { leftHall }).Returns(new[] { rightHall });

            var sidePassages = sidePassageGenerator.Generate(9266, specifications);
            Assert.That(sidePassages.Count(), Is.EqualTo(2));

            var first = sidePassages.First();
            var last = sidePassages.Last();

            Assert.That(first, Is.EqualTo(leftHall));
            Assert.That(first.Descriptions.Single(), Is.EqualTo(SidePassageConstants.Left45DegreesAhead));
            Assert.That(last, Is.EqualTo(rightHall));
            Assert.That(last.Descriptions.Single(), Is.EqualTo(SidePassageConstants.Right45DegreesAhead));
        }

        [Test]
        public void GenerateCrossIntersection()
        {
            mockPercentileSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.SidePassages)).Returns(SidePassageConstants.CrossIntersection);

            var leftHall = new Area();
            var rightHall = new Area();
            mockHallGenerator.SetupSequence(g => g.Generate(9266, specifications)).Returns([leftHall]).Returns([rightHall]);

            var sidePassages = sidePassageGenerator.Generate(9266, specifications);
            Assert.That(sidePassages.Count(), Is.EqualTo(3));

            var first = sidePassages.First();
            var middle = sidePassages.Skip(1).First();
            var last = sidePassages.Last();

            Assert.That(first, Is.EqualTo(leftHall));
            Assert.That(first.Descriptions.Single(), Is.EqualTo(SidePassageConstants.Left90Degrees));
            Assert.That(middle.Type, Is.EqualTo(AreaTypeConstants.Hall));
            Assert.That(middle.Length, Is.EqualTo(30));
            Assert.That(middle.Width, Is.EqualTo(0));
            Assert.That(last, Is.EqualTo(rightHall));
            Assert.That(last.Descriptions.Single(), Is.EqualTo(SidePassageConstants.Right90Degrees));
        }

        [Test]
        public void GenerateXIntersection()
        {
            mockPercentileSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.SidePassages)).Returns(SidePassageConstants.XIntersection);

            var leftBehindHall = new Area();
            var leftAheadHall = new Area();
            var rightAheadHall = new Area();
            var rightBehindHall = new Area();
            mockHallGenerator.SetupSequence(g => g.Generate(9266, specifications))
                .Returns([leftBehindHall]).Returns([leftAheadHall])
                .Returns([rightAheadHall]).Returns([rightBehindHall]);

            var sidePassages = sidePassageGenerator.Generate(9266, specifications).ToArray();
            Assert.That(sidePassages.Length, Is.EqualTo(4));

            Assert.That(sidePassages[0], Is.EqualTo(leftBehindHall));
            Assert.That(sidePassages[0].Descriptions.Single(), Is.EqualTo(SidePassageConstants.Left45DegreesBehind));
            Assert.That(sidePassages[1], Is.EqualTo(leftAheadHall));
            Assert.That(sidePassages[1].Descriptions.Single(), Is.EqualTo(SidePassageConstants.Left45DegreesAhead));
            Assert.That(sidePassages[2], Is.EqualTo(rightAheadHall));
            Assert.That(sidePassages[2].Descriptions.Single(), Is.EqualTo(SidePassageConstants.Right45DegreesAhead));
            Assert.That(sidePassages[3], Is.EqualTo(rightBehindHall));
            Assert.That(sidePassages[3].Descriptions.Single(), Is.EqualTo(SidePassageConstants.Right45DegreesBehind));
        }
    }
}

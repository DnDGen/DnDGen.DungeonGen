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
    public class SidePassageGeneratorTests
    {
        private AreaGenerator sidePassageGenerator;
        private Mock<AreaGenerator> mockHallGenerator;
        private Mock<IPercentileSelector> mockPercentileSelector;

        [SetUp]
        public void Setup()
        {
            mockHallGenerator = new Mock<AreaGenerator>();
            mockPercentileSelector = new Mock<IPercentileSelector>();
            sidePassageGenerator = new SidePassageGenerator(mockPercentileSelector.Object, mockHallGenerator.Object);
        }

        [Test]
        public void GeneratorReturnsOriginalHallwayAndSidePassage()
        {
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.SidePassages)).Returns("description");

            var generatedSidePassage = new Area();
            mockHallGenerator.Setup(g => g.Generate(9266, 90210)).Returns(new[] { generatedSidePassage });

            var sidePassages = sidePassageGenerator.Generate(9266, 90210);
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
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.SidePassages)).Returns(SidePassageConstants.TIntersection);

            var leftHall = new Area();
            var rightHall = new Area();
            mockHallGenerator.SetupSequence(g => g.Generate(9266, 90210)).Returns(new[] { leftHall }).Returns(new[] { rightHall });

            var sidePassages = sidePassageGenerator.Generate(9266, 90210);
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
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.SidePassages)).Returns(SidePassageConstants.YIntersection);

            var leftHall = new Area();
            var rightHall = new Area();
            mockHallGenerator.SetupSequence(g => g.Generate(9266, 90210)).Returns(new[] { leftHall }).Returns(new[] { rightHall });

            var sidePassages = sidePassageGenerator.Generate(9266, 90210);
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
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.SidePassages)).Returns(SidePassageConstants.CrossIntersection);

            var leftHall = new Area();
            var rightHall = new Area();
            mockHallGenerator.SetupSequence(g => g.Generate(9266, 90210)).Returns(new[] { leftHall }).Returns(new[] { rightHall });

            var sidePassages = sidePassageGenerator.Generate(9266, 90210);
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
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.SidePassages)).Returns(SidePassageConstants.XIntersection);

            var leftBehindHall = new Area();
            var leftAheadHall = new Area();
            var rightAheadHall = new Area();
            var rightBehindHall = new Area();
            mockHallGenerator.SetupSequence(g => g.Generate(9266, 90210))
                .Returns(new[] { leftBehindHall }).Returns(new[] { leftAheadHall })
                .Returns(new[] { rightAheadHall }).Returns(new[] { rightBehindHall });

            var sidePassages = sidePassageGenerator.Generate(9266, 90210).ToArray();
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

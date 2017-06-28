using DungeonGen.Domain.Generators.AreaGenerators;
using EncounterGen.Generators;
using EventGen;
using Moq;
using NUnit.Framework;

namespace DungeonGen.Tests.Unit.Generators.AreaGenerators
{
    [TestFixture]
    public class AreaGeneratorEventDecoratorTests
    {
        private AreaGenerator decorator;
        private Mock<AreaGenerator> mockInnerGenerator;
        private Mock<GenEventQueue> mockEventQueue;
        private EncounterSpecifications specifications;

        [SetUp]
        public void Setup()
        {
            mockInnerGenerator = new Mock<AreaGenerator>();
            mockEventQueue = new Mock<GenEventQueue>();

            decorator = new AreaGeneratorEventDecorator(mockInnerGenerator.Object, mockEventQueue.Object);
            specifications = new EncounterSpecifications();

            specifications.Environment = "environment";
            specifications.Level = 90210;
            specifications.Temperature = "temperature";
            specifications.TimeOfDay = "time of day";

            mockInnerGenerator.SetupGet(g => g.AreaType).Returns("area type");
        }

        [Test]
        public void AreaTypeIsInnerAreaType()
        {
            Assert.That(decorator.AreaType, Is.EqualTo("area type"));
        }

        [Test]
        public void ReturnInnerAreas()
        {
            var areas = new[] { new Area(), new Area() };
            mockInnerGenerator.Setup(g => g.Generate(9266, specifications)).Returns(areas);

            var generatedAreas = decorator.Generate(9266, specifications);
            Assert.That(generatedAreas, Is.EqualTo(areas));
        }

        [Test]
        public void LogEventsForAreas()
        {
            var areas = new[] { new Area { Type = "area type" }, new Area { Type = "other area type" } };
            mockInnerGenerator.Setup(g => g.Generate(9266, specifications)).Returns(areas);

            var generatedAreas = decorator.Generate(9266, specifications);
            Assert.That(generatedAreas, Is.EqualTo(areas));
            mockEventQueue.Verify(q => q.Enqueue(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            mockEventQueue.Verify(q => q.Enqueue("DungeonGen", $"Generating {specifications.Description} area type on dungeon level 9266"), Times.Once);
            mockEventQueue.Verify(q => q.Enqueue("DungeonGen", $"Generated 2 areas for area type: [area type, other area type]"), Times.Once);
        }

        [Test]
        public void LogEventsForAreasWithoutTypes()
        {
            var areas = new[] { new Area(), new Area() };
            mockInnerGenerator.Setup(g => g.Generate(9266, specifications)).Returns(areas);

            var generatedAreas = decorator.Generate(9266, specifications);
            Assert.That(generatedAreas, Is.EqualTo(areas));
            mockEventQueue.Verify(q => q.Enqueue(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            mockEventQueue.Verify(q => q.Enqueue("DungeonGen", $"Generating {specifications.Description} area type on dungeon level 9266"), Times.Once);
            mockEventQueue.Verify(q => q.Enqueue("DungeonGen", $"Generated 2 areas for area type"), Times.Once);
        }
    }
}

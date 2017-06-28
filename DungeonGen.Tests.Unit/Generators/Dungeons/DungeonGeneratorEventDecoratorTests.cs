using DungeonGen.Domain.Generators.Dungeons;
using EncounterGen.Generators;
using EventGen;
using Moq;
using NUnit.Framework;

namespace DungeonGen.Tests.Unit.Generators.Dungeons
{
    [TestFixture]
    public class DungeonGeneratorEventDecoratorTests
    {
        private IDungeonGenerator decorator;
        private Mock<IDungeonGenerator> mockInnerGenerator;
        private Mock<GenEventQueue> mockEventQueue;
        private EncounterSpecifications specifications;

        [SetUp]
        public void Setup()
        {
            mockInnerGenerator = new Mock<IDungeonGenerator>();
            mockEventQueue = new Mock<GenEventQueue>();

            decorator = new DungeonGeneratorEventDecorator(mockInnerGenerator.Object, mockEventQueue.Object);
            specifications = new EncounterSpecifications();

            specifications.Environment = "environment";
            specifications.Level = 90210;
            specifications.Temperature = "temperature";
            specifications.TimeOfDay = "time of day";
        }

        [Test]
        public void ReturnInnerAreasFromDoor()
        {
            var areas = new[] { new Area(), new Area() };
            mockInnerGenerator.Setup(g => g.GenerateFromDoor(9266, specifications)).Returns(areas);

            var generatedAreas = decorator.GenerateFromDoor(9266, specifications);
            Assert.That(generatedAreas, Is.EqualTo(areas));
        }

        [Test]
        public void LogEventsForAreasFromDoor()
        {
            var areas = new[] { new Area { Type = "area type" }, new Area { Type = "other area type" } };
            mockInnerGenerator.Setup(g => g.GenerateFromDoor(9266, specifications)).Returns(areas);

            var generatedAreas = decorator.GenerateFromDoor(9266, specifications);
            Assert.That(generatedAreas, Is.EqualTo(areas));
            mockEventQueue.Verify(q => q.Enqueue(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            mockEventQueue.Verify(q => q.Enqueue("DungeonGen", $"Generating dungeon area in {specifications.Description} from door on dungeon level 9266"), Times.Once);
            mockEventQueue.Verify(q => q.Enqueue("DungeonGen", $"Generated 2 areas: [area type, other area type]"), Times.Once);
        }

        [Test]
        public void ReturnInnerAreasFromHall()
        {
            var areas = new[] { new Area(), new Area() };
            mockInnerGenerator.Setup(g => g.GenerateFromHall(9266, specifications)).Returns(areas);

            var generatedAreas = decorator.GenerateFromHall(9266, specifications);
            Assert.That(generatedAreas, Is.EqualTo(areas));
        }

        [Test]
        public void LogEventsForAreasFromHall()
        {
            var areas = new[] { new Area { Type = "area type" }, new Area { Type = "other area type" } };
            mockInnerGenerator.Setup(g => g.GenerateFromHall(9266, specifications)).Returns(areas);

            var generatedAreas = decorator.GenerateFromHall(9266, specifications);
            Assert.That(generatedAreas, Is.EqualTo(areas));
            mockEventQueue.Verify(q => q.Enqueue(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            mockEventQueue.Verify(q => q.Enqueue("DungeonGen", $"Generating dungeon area in {specifications.Description} from hall on dungeon level 9266"), Times.Once);
            mockEventQueue.Verify(q => q.Enqueue("DungeonGen", $"Generated 2 areas: [area type, other area type]"), Times.Once);
        }
    }
}

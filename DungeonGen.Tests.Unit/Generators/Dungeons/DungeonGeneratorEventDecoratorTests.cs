using DungeonGen.Domain.Generators.Dungeons;
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

        [SetUp]
        public void Setup()
        {
            mockInnerGenerator = new Mock<IDungeonGenerator>();
            mockEventQueue = new Mock<GenEventQueue>();

            decorator = new DungeonGeneratorEventDecorator(mockInnerGenerator.Object, mockEventQueue.Object);
        }

        [Test]
        public void ReturnInnerAreasFromDoor()
        {
            var areas = new[] { new Area(), new Area() };
            mockInnerGenerator.Setup(g => g.GenerateFromDoor(9266, 90210, "temperature")).Returns(areas);

            var generatedAreas = decorator.GenerateFromDoor(9266, 90210, "temperature");
            Assert.That(generatedAreas, Is.EqualTo(areas));
        }

        [Test]
        public void LogEventsForAreasFromDoor()
        {
            var areas = new[] { new Area { Type = "area type" }, new Area { Type = "other area type" } };
            mockInnerGenerator.Setup(g => g.GenerateFromDoor(9266, 90210, "temperature")).Returns(areas);

            var generatedAreas = decorator.GenerateFromDoor(9266, 90210, "temperature");
            Assert.That(generatedAreas, Is.EqualTo(areas));
            mockEventQueue.Verify(q => q.Enqueue(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            mockEventQueue.Verify(q => q.Enqueue("DungeonGen", $"Generating temperature dungeon area from door for level 90210 party on dungeon level 9266"), Times.Once);
            mockEventQueue.Verify(q => q.Enqueue("DungeonGen", $"Finished generating 2 areas: [area type, other area type]"), Times.Once);
        }

        [Test]
        public void ReturnInnerAreasFromHall()
        {
            var areas = new[] { new Area(), new Area() };
            mockInnerGenerator.Setup(g => g.GenerateFromHall(9266, 90210, "temperature")).Returns(areas);

            var generatedAreas = decorator.GenerateFromHall(9266, 90210, "temperature");
            Assert.That(generatedAreas, Is.EqualTo(areas));
        }

        [Test]
        public void LogEventsForAreasFromHall()
        {
            var areas = new[] { new Area { Type = "area type" }, new Area { Type = "other area type" } };
            mockInnerGenerator.Setup(g => g.GenerateFromHall(9266, 90210, "temperature")).Returns(areas);

            var generatedAreas = decorator.GenerateFromHall(9266, 90210, "temperature");
            Assert.That(generatedAreas, Is.EqualTo(areas));
            mockEventQueue.Verify(q => q.Enqueue(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            mockEventQueue.Verify(q => q.Enqueue("DungeonGen", $"Generating temperature dungeon area from hall for level 90210 party on dungeon level 9266"), Times.Once);
            mockEventQueue.Verify(q => q.Enqueue("DungeonGen", $"Finished generating 2 areas: [area type, other area type]"), Times.Once);
        }
    }
}

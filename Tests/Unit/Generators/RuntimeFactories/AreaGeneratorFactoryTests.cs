using DungeonGen.Common;
using DungeonGen.Generators;
using DungeonGen.Generators.Domain.RuntimeFactories;
using DungeonGen.Generators.Domain.RuntimeFactories.Domain;
using Moq;
using NUnit.Framework;

namespace DungeonGen.Tests.Unit.Generators.RuntimeFactories
{
    [TestFixture]
    public class AreaGeneratorFactoryTests
    {
        private IAreaGeneratorFactory areaGeneratorFactory;
        private Mock<AreaGenerator> mockChamberGenerator;
        private Mock<AreaGenerator> mockDoorGenerator;
        private Mock<AreaGenerator> mockRoomGenerator;
        private Mock<AreaGenerator> mockSidePassageGenerator;
        private Mock<AreaGenerator> mockStairsGenerator;
        private Mock<AreaGenerator> mockTurnGenerator;

        [SetUp]
        public void Setup()
        {
            mockChamberGenerator = new Mock<AreaGenerator>();
            mockDoorGenerator = new Mock<AreaGenerator>();
            mockRoomGenerator = new Mock<AreaGenerator>();
            mockSidePassageGenerator = new Mock<AreaGenerator>();
            mockStairsGenerator = new Mock<AreaGenerator>();
            mockTurnGenerator = new Mock<AreaGenerator>();

            areaGeneratorFactory = new AreaGeneratorFactory(mockChamberGenerator.Object, mockDoorGenerator.Object, mockRoomGenerator.Object,
                mockSidePassageGenerator.Object, mockStairsGenerator.Object, mockTurnGenerator.Object);
        }

        [Test]
        public void BuildChamberGenerator()
        {
            var generator = areaGeneratorFactory.Build(AreaTypeConstants.Chamber);
            Assert.That(generator, Is.Not.Null);
            Assert.That(generator, Is.EqualTo(mockChamberGenerator.Object));
        }

        [Test]
        public void BuildDoorGenerator()
        {
            var generator = areaGeneratorFactory.Build(AreaTypeConstants.Door);
            Assert.That(generator, Is.Not.Null);
            Assert.That(generator, Is.EqualTo(mockDoorGenerator.Object));
        }

        [Test]
        public void BuildRoomGenerator()
        {
            var generator = areaGeneratorFactory.Build(AreaTypeConstants.Room);
            Assert.That(generator, Is.Not.Null);
            Assert.That(generator, Is.EqualTo(mockRoomGenerator.Object));
        }

        [Test]
        public void BuildSidePassageGenerator()
        {
            var generator = areaGeneratorFactory.Build(AreaTypeConstants.SidePassage);
            Assert.That(generator, Is.Not.Null);
            Assert.That(generator, Is.EqualTo(mockSidePassageGenerator.Object));
        }

        [Test]
        public void BuildStairsGenerator()
        {
            var generator = areaGeneratorFactory.Build(AreaTypeConstants.Stairs);
            Assert.That(generator, Is.Not.Null);
            Assert.That(generator, Is.EqualTo(mockStairsGenerator.Object));
        }

        [Test]
        public void BuildTurnGenerator()
        {
            var generator = areaGeneratorFactory.Build(AreaTypeConstants.Turn);
            Assert.That(generator, Is.Not.Null);
            Assert.That(generator, Is.EqualTo(mockTurnGenerator.Object));
        }

        [TestCase(AreaTypeConstants.Cave, false)]
        [TestCase(AreaTypeConstants.Chamber, true)]
        [TestCase(AreaTypeConstants.DeadEnd, false)]
        [TestCase(AreaTypeConstants.Door, true)]
        [TestCase(AreaTypeConstants.General, false)]
        [TestCase(AreaTypeConstants.Hall, false)]
        [TestCase(AreaTypeConstants.Room, true)]
        [TestCase(AreaTypeConstants.SidePassage, true)]
        [TestCase(AreaTypeConstants.Special, false)]
        [TestCase(AreaTypeConstants.Stairs, true)]
        [TestCase(AreaTypeConstants.Turn, true)]
        [TestCase("area type", false)]
        public void AreaTypeHasSpecificGenerator(string areaType, bool hasSpecificGenerator)
        {
            Assert.That(areaGeneratorFactory.HasSpecificGenerator(areaType), Is.EqualTo(hasSpecificGenerator));
        }
    }
}

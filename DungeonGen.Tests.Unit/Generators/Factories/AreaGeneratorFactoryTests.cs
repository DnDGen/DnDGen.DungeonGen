using DungeonGen.Domain.Generators.AreaGenerators;
using DungeonGen.Domain.Generators.Factories;
using Moq;
using NUnit.Framework;

namespace DungeonGen.Tests.Unit.Generators.Factories
{
    [TestFixture]
    public class AreaGeneratorFactoryTests
    {
        private AreaGeneratorFactory areaGeneratorFactory;
        private Mock<JustInTimeFactory> mockJustInTimeFactory;

        [SetUp]
        public void Setup()
        {
            mockJustInTimeFactory = new Mock<JustInTimeFactory>();
            areaGeneratorFactory = new DomainAreaGeneratorFactory(mockJustInTimeFactory.Object);
        }

        [Test]
        public void BuildGenerator()
        {
            var mockGenerator = new Mock<AreaGenerator>();
            mockJustInTimeFactory.Setup(f => f.Build<AreaGenerator>("area type")).Returns(mockGenerator.Object);

            var generator = areaGeneratorFactory.Build("area type");
            Assert.That(generator, Is.Not.Null);
            Assert.That(generator, Is.EqualTo(mockGenerator.Object));
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
        [TestCase(SidePassageConstants.ParallelPassage, true)]
        [TestCase("area type", false)]
        public void AreaTypeHasSpecificGenerator(string areaType, bool hasSpecificGenerator)
        {
            Assert.That(areaGeneratorFactory.HasSpecificGenerator(areaType), Is.EqualTo(hasSpecificGenerator));
        }
    }
}

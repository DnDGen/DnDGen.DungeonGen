using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.AreaGenerators;
using DungeonGen.Domain.Generators.Factories;
using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace DungeonGen.Tests.Unit.Generators.AreaGenerators
{
    [TestFixture]
    public class SpecialAreaGeneratorTests
    {
        private AreaGenerator specialAreaGenerator;
        private Mock<IAreaPercentileSelector> mockAreaPercentileSelector;
        private Mock<IPercentileSelector> mockPercentileSelector;
        private Mock<PoolGenerator> mockPoolGenerator;
        private Mock<AreaGenerator> mockCaveGenerator;
        private Mock<AreaGeneratorFactory> mockAreaGeneratorFactory;

        [SetUp]
        public void Setup()
        {
            mockAreaPercentileSelector = new Mock<IAreaPercentileSelector>();
            mockPercentileSelector = new Mock<IPercentileSelector>();
            mockPoolGenerator = new Mock<PoolGenerator>();
            mockCaveGenerator = new Mock<AreaGenerator>();
            mockAreaGeneratorFactory = new Mock<AreaGeneratorFactory>();
            specialAreaGenerator = new SpecialAreaGenerator(mockAreaPercentileSelector.Object, mockPercentileSelector.Object, mockPoolGenerator.Object, mockAreaGeneratorFactory.Object);

            mockAreaGeneratorFactory.Setup(f => f.Build(AreaTypeConstants.Cave)).Returns(mockCaveGenerator.Object);
        }

        [Test]
        public void AreaTypeIsSpecial()
        {
            Assert.That(specialAreaGenerator.AreaType, Is.EqualTo(AreaTypeConstants.Special));
        }

        [Test]
        public void GenerateSpecialArea()
        {
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.SpecialAreaShapes)).Returns("dodecahedron");

            var size = new Area { Length = 90210 };
            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.SpecialAreaSizes)).Returns(size);

            var area = specialAreaGenerator.Generate(9266, 600, "temperature").Single();
            Assert.That(area.Descriptions.Single(), Is.EqualTo("dodecahedron"));
            Assert.That(area.Length, Is.EqualTo(90210));
            Assert.That(area.Width, Is.EqualTo(1));
        }

        [Test]
        public void GenerateBigSpecialArea()
        {
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.SpecialAreaShapes)).Returns("dodecahedron");

            var biggerSize = new Area { Width = 42 };
            var size = new Area { Length = 90210 };
            mockAreaPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.SpecialAreaSizes)).Returns(biggerSize).Returns(size);

            var area = specialAreaGenerator.Generate(9266, 600, "temperature").Single();
            Assert.That(area.Descriptions.Single(), Is.EqualTo("dodecahedron"));
            Assert.That(area.Length, Is.EqualTo(90252));
            Assert.That(area.Width, Is.EqualTo(1));
        }

        [Test]
        public void GeneratePoolForCircularRooms()
        {
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.SpecialAreaShapes)).Returns(DescriptionConstants.Circular);

            var size = new Area { Length = 90210 };
            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.SpecialAreaSizes)).Returns(size);

            var pool = new Pool();
            mockPoolGenerator.Setup(g => g.Generate(600, "temperature")).Returns(pool);

            var area = specialAreaGenerator.Generate(9266, 600, "temperature").Single();
            Assert.That(area.Descriptions.Single(), Is.EqualTo(DescriptionConstants.Circular));
            Assert.That(area.Length, Is.EqualTo(90210));
            Assert.That(area.Width, Is.EqualTo(1));
            Assert.That(area.Contents.Pool, Is.EqualTo(pool));
        }

        [Test]
        public void GenerateNoPoolForNonCircularRooms()
        {
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.SpecialAreaShapes)).Returns("dodecahedron");

            var size = new Area { Length = 90210 };
            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.SpecialAreaSizes)).Returns(size);

            var pool = new Pool();
            mockPoolGenerator.Setup(g => g.Generate(9266, "temperature")).Returns(pool);

            var area = specialAreaGenerator.Generate(9266, 600, "temperature").Single();
            Assert.That(area.Descriptions.Single(), Is.EqualTo("dodecahedron"));
            Assert.That(area.Length, Is.EqualTo(90210));
            Assert.That(area.Width, Is.EqualTo(1));
            Assert.That(area.Contents.Pool, Is.Null);
        }

        [Test]
        public void GenerateNoPoolForCircularRooms()
        {
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.SpecialAreaShapes)).Returns(DescriptionConstants.Circular);

            var size = new Area { Length = 90210 };
            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.SpecialAreaSizes)).Returns(size);

            Pool noPool = null;
            mockPoolGenerator.Setup(g => g.Generate(9266, "temperature")).Returns(noPool);

            var area = specialAreaGenerator.Generate(9266, 600, "temperature").Single();
            Assert.That(area.Descriptions.Single(), Is.EqualTo(DescriptionConstants.Circular));
            Assert.That(area.Length, Is.EqualTo(90210));
            Assert.That(area.Width, Is.EqualTo(1));
            Assert.That(area.Contents.Pool, Is.Null);
        }

        [Test]
        public void GenerateCave()
        {
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.SpecialAreaShapes)).Returns(AreaTypeConstants.Cave);

            var cave = new Area();
            var otherCave = new Area();
            mockCaveGenerator.Setup(g => g.Generate(9266, 600, "temperature")).Returns(new[] { cave, otherCave });

            var areas = specialAreaGenerator.Generate(9266, 600, "temperature");
            Assert.That(areas.Count(), Is.EqualTo(2));
            Assert.That(areas.First(), Is.EqualTo(cave));
            Assert.That(areas.Last(), Is.EqualTo(otherCave));
        }
    }
}

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
    public class HallGeneratorTests
    {
        private AreaGenerator hallGenerator;
        private Mock<IAreaPercentileSelector> mockAreaPercentileSelector;

        [SetUp]
        public void Setup()
        {
            mockAreaPercentileSelector = new Mock<IAreaPercentileSelector>();
            hallGenerator = new HallGenerator();
        }

        [Test]
        public void GenerateHallReturnsSomething()
        {
            var halls = hallGenerator.Generate(9266);
            Assert.That(halls, Is.Not.Null);
            Assert.That(halls, Is.Not.Empty);
            Assert.That(halls.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GenerateHall()
        {
            var selectedHall = new Area();
            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.PassageWidths)).Returns(selectedHall);

            var hall = hallGenerator.Generate(9266).Single();
            Assert.That(hall, Is.EqualTo(selectedHall));
        }

        [Test]
        public void GenerateSpecialHall()
        {
            var selectedHall = new Area();
            selectedHall.Type = AreaTypeConstants.Special;

            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.PassageWidths)).Returns(selectedHall);

            var hall = hallGenerator.Generate(9266).Single();
            Assert.That(hall, Is.EqualTo(selectedHall));
        }
    }
}

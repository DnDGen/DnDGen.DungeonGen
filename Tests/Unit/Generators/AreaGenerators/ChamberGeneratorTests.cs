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
    public class ChamberGeneratorTests
    {
        private AreaGenerator chamberGenerator;
        private Mock<IAreaPercentileSelector> mockAreaPercentileSelector;
        private Area selectedChamber;
        private Mock<AreaGenerator> mockSpecialChamberGenerator;
        private Mock<ExitGenerator> mockExitGenerator;
        private Mock<ContentsGenerator> mockContentsGenerator;

        [SetUp]
        public void Setup()
        {
            mockAreaPercentileSelector = new Mock<IAreaPercentileSelector>();
            mockSpecialChamberGenerator = new Mock<AreaGenerator>();
            mockExitGenerator = new Mock<ExitGenerator>();
            mockContentsGenerator = new Mock<ContentsGenerator>();
            chamberGenerator = new ChamberGenerator(mockAreaPercentileSelector.Object, mockSpecialChamberGenerator.Object, mockExitGenerator.Object, mockContentsGenerator.Object);

            selectedChamber = new Area();
            selectedChamber.Length = 9266;
            selectedChamber.Width = 90210;

            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.Chambers)).Returns(selectedChamber);
        }

        [Test]
        public void GenerateChamber()
        {
            var chambers = chamberGenerator.Generate(42);
            Assert.That(chambers, Is.Not.Null);
            Assert.That(chambers, Is.Not.Empty);
        }

        [Test]
        public void GenerateChamberFromSelector()
        {
            var chambers = chamberGenerator.Generate(42);
            Assert.That(chambers.Single(), Is.EqualTo(selectedChamber));
        }

        [Test]
        public void GenerateChamberExits()
        {
            var firstExit = new Area();
            var secondExit = new Area();
            mockExitGenerator.Setup(g => g.Generate(9266, 90210)).Returns(new[] { firstExit, secondExit });

            var chambers = chamberGenerator.Generate(42);
            Assert.That(chambers, Contains.Item(selectedChamber));
            Assert.That(chambers, Contains.Item(firstExit));
            Assert.That(chambers, Contains.Item(secondExit));
        }

        [Test]
        public void GenerateChamberContents()
        {
            var contents = new Contents();
            mockContentsGenerator.Setup(g => g.Generate(42)).Returns(contents);

            var chambers = chamberGenerator.Generate(42);
            Assert.That(chambers.Single().Contents, Is.EqualTo(contents));
        }

        [Test]
        public void GenerateSpecialChamber()
        {
            selectedChamber.Type = AreaTypeConstants.Special;
            var firstSpecialChamber = new Area();
            var secondSpecialArea = new Area();
            var specialChambers = new[] { firstSpecialChamber, secondSpecialArea };

            mockSpecialChamberGenerator.Setup(g => g.Generate(42)).Returns(specialChambers);

            var chambers = chamberGenerator.Generate(42);
            Assert.That(chambers, Is.EqualTo(specialChambers));
        }
    }
}

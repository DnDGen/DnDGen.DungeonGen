using DungeonGen.Common;
using DungeonGen.Generators;
using DungeonGen.Generators.Domain.AreaGenerators;
using DungeonGen.Selectors;
using DungeonGen.Tables;
using Moq;
using NUnit.Framework;
using System;

namespace DungeonGen.Tests.Unit.Generators.AreaGenerators
{
    [TestFixture]
    public class ChamberGeneratorTests
    {
        private AreaGenerator chamberGenerator;
        private Mock<IAreaPercentileSelector> mockAreaPercentileSelector;
        private Area selectedChamber;

        [SetUp]
        public void Setup()
        {
            mockAreaPercentileSelector = new Mock<IAreaPercentileSelector>();
            chamberGenerator = new ChamberGenerator();

            selectedChamber = new Area();
            selectedChamber.Length = 9266;
            selectedChamber.Width = 90210;

            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.Chambers)).Returns(selectedChamber);
        }

        [Test]
        public void GenerateChamber()
        {
            var chamber = chamberGenerator.Generate(9266);
            Assert.That(chamber, Is.Not.Null);
        }

        [Test]
        public void GenerateChamberFromSelector()
        {
            var chamber = chamberGenerator.Generate(9266);
            Assert.That(chamber, Is.EqualTo(selectedChamber));
        }

        [Test]
        public void GenerateSpecialChamber()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void GenerateChamberExits()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void GenerateChamberContents()
        {
            throw new NotImplementedException();
        }
    }
}

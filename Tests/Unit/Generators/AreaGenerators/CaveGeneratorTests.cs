using DungeonGen.Common;
using DungeonGen.Generators;
using DungeonGen.Generators.Domain.AreaGenerators;
using DungeonGen.Selectors;
using DungeonGen.Tables;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace DungeonGen.Tests.Unit.Generators.AreaGenerators
{
    [TestFixture]
    public class CaveGeneratorTests
    {
        private AreaGenerator caveGenerator;
        private Mock<IAreaPercentileSelector> mockAreaPercentileSelector;
        private Area selectedCave;

        [SetUp]
        public void Setup()
        {
            mockAreaPercentileSelector = new Mock<IAreaPercentileSelector>();
            caveGenerator = new CaveGenerator();
            selectedCave = new Area();

            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.Caves)).Returns(selectedCave);
        }

        [Test]
        public void ReturnCave()
        {
            var caves = caveGenerator.Generate(9266, 90210);
            Assert.That(caves, Is.Not.Null);
            Assert.That(caves, Is.Not.Empty);
        }

        [Test]
        public void GenerateCave()
        {
            var caves = caveGenerator.Generate(9266, 90210);
            Assert.That(caves.Single(), Is.EqualTo(selectedCave));
        }

        [Test]
        public void GenerateDoubleCave()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void GenerateCaveWithPool()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void GenerateCaveWithLake()
        {
            throw new NotImplementedException();
        }
    }
}

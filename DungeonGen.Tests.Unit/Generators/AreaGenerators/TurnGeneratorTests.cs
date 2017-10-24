using DnDGen.Core.Selectors.Percentiles;
using DungeonGen.Domain.Generators.AreaGenerators;
using DungeonGen.Domain.Tables;
using EncounterGen.Generators;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace DungeonGen.Tests.Unit.Generators.AreaGenerators
{
    [TestFixture]
    public class TurnGeneratorTests
    {
        private AreaGenerator turnGenerator;
        private Mock<IPercentileSelector> mockPercentileSelector;
        private EncounterSpecifications specifications;

        [SetUp]
        public void Setup()
        {
            mockPercentileSelector = new Mock<IPercentileSelector>();
            turnGenerator = new TurnGenerator(mockPercentileSelector.Object);
            specifications = new EncounterSpecifications();
        }

        [Test]
        public void AreaTypeIsTurn()
        {
            Assert.That(turnGenerator.AreaType, Is.EqualTo(AreaTypeConstants.Turn));
        }

        [Test]
        public void GenerateTurn()
        {
            var turns = turnGenerator.Generate(9266, specifications);
            Assert.That(turns, Is.Not.Null);
            Assert.That(turns, Is.Not.Empty);
            Assert.That(turns.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GetTurnDescription()
        {
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.Turns)).Returns("loop the loop");

            var turn = turnGenerator.Generate(9266, specifications).Single();
            Assert.That(turn.Contents.IsEmpty, Is.True);
            Assert.That(turn.Descriptions.Single(), Is.EqualTo("loop the loop"));
            Assert.That(turn.Length, Is.EqualTo(30));
            Assert.That(turn.Type, Is.EqualTo(AreaTypeConstants.Turn));
            Assert.That(turn.Width, Is.EqualTo(0));
        }
    }
}

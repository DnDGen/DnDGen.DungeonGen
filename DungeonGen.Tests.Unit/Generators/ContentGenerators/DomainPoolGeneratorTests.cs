using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.ContentGenerators;
using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using EncounterGen.Common;
using EncounterGen.Generators;
using Moq;
using NUnit.Framework;
using TreasureGen;
using TreasureGen.Generators;

namespace DungeonGen.Tests.Unit.Generators.ContentGenerators
{
    [TestFixture]
    public class DomainPoolGeneratorTests
    {
        private PoolGenerator poolGenerator;
        private Mock<IPercentileSelector> mockPercentileSelector;
        private Mock<IEncounterGenerator> mockEncounterGenerator;
        private Mock<ITreasureGenerator> mockTreasureGenerator;
        private string selectedPool;
        private Encounter encounter;
        private Treasure treasure;

        [SetUp]
        public void Setup()
        {
            mockPercentileSelector = new Mock<IPercentileSelector>();
            mockEncounterGenerator = new Mock<IEncounterGenerator>();
            mockTreasureGenerator = new Mock<ITreasureGenerator>();
            poolGenerator = new DomainPoolGenerator(mockPercentileSelector.Object, mockEncounterGenerator.Object, mockTreasureGenerator.Object);

            selectedPool = string.Empty;
            encounter = new Encounter();
            treasure = new Treasure();

            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.Pools)).Returns(() => selectedPool);
            mockEncounterGenerator.Setup(g => g.Generate(EnvironmentConstants.Dungeon, 9266)).Returns(encounter);
            mockTreasureGenerator.Setup(g => g.GenerateAtLevel(9266)).Returns(treasure);
        }

        [Test]
        public void GenerateNoPool()
        {
            var pool = poolGenerator.Generate(9266);
            Assert.That(pool, Is.Null);
        }

        [Test]
        public void GeneratePool()
        {
            selectedPool = "swimming pool";

            var pool = poolGenerator.Generate(9266);
            Assert.That(pool, Is.Not.Null);
            Assert.That(pool.Encounter, Is.Null);
            Assert.That(pool.MagicPower, Is.Empty);
            Assert.That(pool.Treasure, Is.Null);
        }

        [Test]
        public void GeneratePoolWithEncounter()
        {
            selectedPool = ContentsTypeConstants.Encounter;

            var pool = poolGenerator.Generate(9266);
            Assert.That(pool, Is.Not.Null);
            Assert.That(pool.Encounter, Is.EqualTo(encounter));
            Assert.That(pool.MagicPower, Is.Empty);
            Assert.That(pool.Treasure, Is.Null);
        }

        [Test]
        public void GeneratePoolWithTreasureAndEncounter()
        {
            selectedPool = ContentsTypeConstants.Encounter + "/" + ContentsTypeConstants.Treasure;
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.TreasureContainers)).Returns("fannypack");

            var pool = poolGenerator.Generate(9266);
            Assert.That(pool, Is.Not.Null);
            Assert.That(pool.Encounter, Is.EqualTo(encounter));
            Assert.That(pool.MagicPower, Is.Empty);
            Assert.That(pool.Treasure.Treasure, Is.EqualTo(treasure));
            Assert.That(pool.Treasure.Container, Is.EqualTo("fannypack"));
        }

        [Test]
        public void GenerateMagicPool()
        {
            selectedPool = ContentsConstants.MagicPool;

            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.MagicPoolPowers)).Returns("grant wishes");

            var pool = poolGenerator.Generate(9266);
            Assert.That(pool, Is.Not.Null);
            Assert.That(pool.Encounter, Is.Null);
            Assert.That(pool.MagicPower, Is.EqualTo("grant wishes"));
            Assert.That(pool.Treasure, Is.Null);
        }

        [Test]
        public void GenerateMagicPoolWithAlignment()
        {
            selectedPool = ContentsConstants.MagicPool;

            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.MagicPoolPowers)).Returns(ContentsConstants.LikesAlignment);
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.MagicPoolAlignments)).Returns("buttheady");

            var pool = poolGenerator.Generate(9266);
            Assert.That(pool, Is.Not.Null);
            Assert.That(pool.Encounter, Is.Null);
            Assert.That(pool.MagicPower, Is.EqualTo("Talking pool (grants wish to buttheady characters, deals 1d20 points of damage to anyone else who speaks to it)"));
            Assert.That(pool.Treasure, Is.Null);
        }

        [Test]
        public void GenerateMagicTeleportingPool()
        {
            selectedPool = ContentsConstants.MagicPool;

            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.MagicPoolPowers)).Returns(ContentsConstants.TeleportationPool);
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.MagicPoolTeleportationDestinations)).Returns("to your mom's house");

            var pool = poolGenerator.Generate(9266);
            Assert.That(pool, Is.Not.Null);
            Assert.That(pool.Encounter, Is.Null);
            Assert.That(pool.MagicPower, Is.EqualTo("Wading into the pool teleports the character to your mom's house"));
            Assert.That(pool.Treasure, Is.Null);
        }
    }
}

using DnDGen.DungeonGen.Generators;
using DnDGen.DungeonGen.Generators.ContentGenerators;
using DnDGen.DungeonGen.Models;
using DnDGen.DungeonGen.Selectors;
using DnDGen.DungeonGen.Tables;
using Moq;
using NUnit.Framework;

namespace DnDGen.DungeonGen.Tests.Unit.Generators.ContentGenerators
{
    [TestFixture]
    public class TrapGeneratorTests
    {
        private ITrapGenerator trapGenerator;
        private Mock<IAreaPercentileSelector> mockAreaPercentileSelector;
        private Area selectedLowLevelTrap;
        private Area selectedMidLevelTrap;
        private Area selectedHighLevelTrap;

        [SetUp]
        public void Setup()
        {
            mockAreaPercentileSelector = new Mock<IAreaPercentileSelector>();
            trapGenerator = new TrapGenerator(mockAreaPercentileSelector.Object);
            selectedLowLevelTrap = new Area();
            selectedMidLevelTrap = new Area();
            selectedHighLevelTrap = new Area();

            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.LowLevelTraps)).Returns(selectedLowLevelTrap);
            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.MidLevelTraps)).Returns(selectedMidLevelTrap);
            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.HighLevelTraps)).Returns(selectedHighLevelTrap);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void GenerateLowLevelTrap(int partyLevel)
        {
            selectedLowLevelTrap.Type = "trap of trappiness";
            selectedLowLevelTrap.Length = 42;
            selectedLowLevelTrap.Width = 600;
            selectedLowLevelTrap.Contents.Miscellaneous = new[] { "90210" };
            selectedLowLevelTrap.Descriptions = new[] { "this trap will trap you", "forEVA" };

            var trap = trapGenerator.Generate(partyLevel);
            Assert.That(trap.ChallengeRating, Is.EqualTo(90210));
            Assert.That(trap.Name, Is.EqualTo("trap of trappiness"));
            Assert.That(trap.SearchDC, Is.EqualTo(42));
            Assert.That(trap.DisableDeviceDC, Is.EqualTo(600));
            Assert.That(trap.Descriptions, Is.EquivalentTo(selectedLowLevelTrap.Descriptions));
        }

        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        public void GenerateMidLevelTrap(int partyLevel)
        {
            selectedMidLevelTrap.Type = "trap of trappiness";
            selectedMidLevelTrap.Length = 42;
            selectedMidLevelTrap.Width = 600;
            selectedMidLevelTrap.Contents.Miscellaneous = new[] { "90210" };
            selectedMidLevelTrap.Descriptions = new[] { "this trap will trap you", "forEVA" };

            var trap = trapGenerator.Generate(partyLevel);
            Assert.That(trap.ChallengeRating, Is.EqualTo(90210));
            Assert.That(trap.Name, Is.EqualTo("trap of trappiness"));
            Assert.That(trap.SearchDC, Is.EqualTo(42));
            Assert.That(trap.DisableDeviceDC, Is.EqualTo(600));
            Assert.That(trap.Descriptions, Is.EquivalentTo(selectedMidLevelTrap.Descriptions));
        }

        [TestCase(13)]
        [TestCase(14)]
        [TestCase(15)]
        [TestCase(16)]
        [TestCase(17)]
        [TestCase(18)]
        [TestCase(19)]
        [TestCase(20)]
        public void GenerateHighLevelTrap(int partyLevel)
        {
            selectedHighLevelTrap.Type = "trap of trappiness";
            selectedHighLevelTrap.Length = 42;
            selectedHighLevelTrap.Width = 600;
            selectedHighLevelTrap.Contents.Miscellaneous = new[] { "90210" };
            selectedHighLevelTrap.Descriptions = new[] { "this trap will trap you", "forEVA" };

            var trap = trapGenerator.Generate(partyLevel);
            Assert.That(trap.ChallengeRating, Is.EqualTo(90210));
            Assert.That(trap.Name, Is.EqualTo("trap of trappiness"));
            Assert.That(trap.SearchDC, Is.EqualTo(42));
            Assert.That(trap.DisableDeviceDC, Is.EqualTo(600));
            Assert.That(trap.Descriptions, Is.EquivalentTo(selectedHighLevelTrap.Descriptions));
        }
    }
}

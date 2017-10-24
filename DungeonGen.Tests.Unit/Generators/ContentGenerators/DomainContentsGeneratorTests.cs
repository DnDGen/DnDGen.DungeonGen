using DnDGen.Core.Generators;
using DnDGen.Core.Selectors.Percentiles;
using DungeonGen.Domain.Generators.ContentGenerators;
using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using Moq;
using NUnit.Framework;
using System.Linq;
using TreasureGen;
using TreasureGen.Generators;

namespace DungeonGen.Tests.Unit.Generators.ContentGenerators
{
    [TestFixture]
    public class DomainContentsGeneratorTests
    {
        private ContentsGenerator contentsGenerator;
        private Mock<IAreaPercentileSelector> mockAreaPercentileSelector;
        private Area selectedContents;
        private Mock<IPercentileSelector> mockPercentileSelector;
        private Mock<ITreasureGenerator> mockTreasureGenerator;
        private Mock<JustInTimeFactory> mockJustInTimeFactory;

        [SetUp]
        public void Setup()
        {
            mockAreaPercentileSelector = new Mock<IAreaPercentileSelector>();
            mockPercentileSelector = new Mock<IPercentileSelector>();
            mockTreasureGenerator = new Mock<ITreasureGenerator>();
            mockJustInTimeFactory = new Mock<JustInTimeFactory>();
            contentsGenerator = new DomainContentsGenerator(mockAreaPercentileSelector.Object, mockPercentileSelector.Object, mockJustInTimeFactory.Object);
            selectedContents = new Area();

            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.Contents)).Returns(selectedContents);
            mockJustInTimeFactory.Setup(f => f.Build<ITreasureGenerator>()).Returns(mockTreasureGenerator.Object);
        }

        [Test]
        public void ReturnContents()
        {
            var contents = contentsGenerator.Generate(9266);
            Assert.That(contents, Is.Not.Null);
        }

        [Test]
        public void GenerateNoContents()
        {
            var contents = contentsGenerator.Generate(9266);
            Assert.That(contents.IsEmpty, Is.True);
        }

        [Test]
        public void GenerateContents()
        {
            selectedContents.Contents.Miscellaneous = new[] { "stuff" };

            var contents = contentsGenerator.Generate(9266);
            Assert.That(contents.IsEmpty, Is.False);
            Assert.That(contents.Miscellaneous.Single(), Is.EqualTo("stuff"));
        }

        [Test]
        public void GenerateMinorFeatures()
        {
            selectedContents.Length = 600;

            var count = 1;
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.MinorFeatures)).Returns(() => $"minor feature {count++}");

            var contents = contentsGenerator.Generate(9266);
            Assert.That(contents.IsEmpty, Is.False);
            Assert.That(contents.Miscellaneous.Count(), Is.EqualTo(600));

            for (var i = 1; i <= 600; i++)
            {
                Assert.That(contents.Miscellaneous, Contains.Item($"minor feature {i}"));
            }
        }

        [Test]
        public void GenerateMajorFeatures()
        {
            selectedContents.Width = 42;

            var count = 1;
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.MajorFeatures)).Returns(() => $"major feature {count++}");

            var contents = contentsGenerator.Generate(9266);
            Assert.That(contents.IsEmpty, Is.False);
            Assert.That(contents.Miscellaneous.Count(), Is.EqualTo(42));

            for (var i = 1; i <= 42; i++)
            {
                Assert.That(contents.Miscellaneous, Contains.Item($"major feature {i}"));
            }
        }

        [Test]
        public void GenerateMinorAndMajorFeatures()
        {
            selectedContents.Length = 600;
            selectedContents.Width = 42;

            var count = 1;
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.MinorFeatures)).Returns(() => $"minor feature {count++}");
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.MajorFeatures)).Returns(() => $"major feature {count++}");

            var contents = contentsGenerator.Generate(9266);
            Assert.That(contents.IsEmpty, Is.False);
            Assert.That(contents.Miscellaneous.Count(), Is.EqualTo(642));

            for (var i = 1; i <= 600; i++)
            {
                Assert.That(contents.Miscellaneous, Contains.Item($"minor feature {i}"));
            }

            for (var i = 601; i <= 642; i++)
            {
                Assert.That(contents.Miscellaneous, Contains.Item($"major feature {i}"));
            }
        }

        [Test]
        public void CanHaveDuplicateFeatures()
        {
            selectedContents.Length = 90210;
            selectedContents.Width = 42;

            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.MinorFeatures)).Returns("minor feature");
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.MajorFeatures)).Returns("major feature");

            var contents = contentsGenerator.Generate(9266);
            Assert.That(contents.IsEmpty, Is.False);
            Assert.That(contents.Miscellaneous.Count(), Is.EqualTo(90252));
            Assert.That(contents.Miscellaneous.Count(m => m == "minor feature"), Is.EqualTo(90210));
            Assert.That(contents.Miscellaneous.Count(m => m == "major feature"), Is.EqualTo(42));
        }

        [Test]
        public void GenerateUncontainedTreasure()
        {
            selectedContents.Contents.Miscellaneous = new[] { ContentsTypeConstants.Treasure };

            var generatedTreasure = new Treasure();
            mockTreasureGenerator.Setup(g => g.GenerateAtLevel(9266)).Returns(generatedTreasure);

            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.TreasureContainers)).Returns(string.Empty);
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.TreasureConcealment)).Returns(string.Empty);

            var contents = contentsGenerator.Generate(9266);
            Assert.That(contents.IsEmpty, Is.False);
            Assert.That(contents.Miscellaneous.Single(), Is.EqualTo(ContentsTypeConstants.Treasure));

            var treasure = contents.Treasures.Single();
            Assert.That(treasure.Treasure, Is.EqualTo(generatedTreasure));
            Assert.That(treasure.Container, Is.Empty);
            Assert.That(treasure.Concealment, Is.Empty);
        }

        [Test]
        public void GenerateContainedTreasure()
        {
            selectedContents.Contents.Miscellaneous = new[] { ContentsTypeConstants.Treasure };

            var generatedTreasure = new Treasure();
            mockTreasureGenerator.Setup(g => g.GenerateAtLevel(9266)).Returns(generatedTreasure);

            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.TreasureContainers)).Returns("container");
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.TreasureConcealment)).Returns(string.Empty);

            var contents = contentsGenerator.Generate(9266);
            Assert.That(contents.IsEmpty, Is.False);
            Assert.That(contents.Miscellaneous.Single(), Is.EqualTo(ContentsTypeConstants.Treasure));

            var treasure = contents.Treasures.Single();
            Assert.That(treasure.Treasure, Is.EqualTo(generatedTreasure));
            Assert.That(treasure.Container, Is.EqualTo("container"));
            Assert.That(treasure.Concealment, Is.Empty);
        }

        [Test]
        public void GenerateHiddenTreasure()
        {
            selectedContents.Contents.Miscellaneous = new[] { ContentsTypeConstants.Treasure };

            var generatedTreasure = new Treasure();
            mockTreasureGenerator.Setup(g => g.GenerateAtLevel(9266)).Returns(generatedTreasure);

            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.TreasureContainers)).Returns(string.Empty);
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.TreasureConcealment)).Returns("peek-a-boo");

            var contents = contentsGenerator.Generate(9266);
            Assert.That(contents.IsEmpty, Is.False);
            Assert.That(contents.Miscellaneous.Single(), Is.EqualTo(ContentsTypeConstants.Treasure));

            var treasure = contents.Treasures.Single();
            Assert.That(treasure.Treasure, Is.EqualTo(generatedTreasure));
            Assert.That(treasure.Container, Is.Empty);
            Assert.That(treasure.Concealment, Is.EqualTo("peek-a-boo"));
        }

        [Test]
        public void GenerateHiddenContainedTreasure()
        {
            selectedContents.Contents.Miscellaneous = new[] { ContentsTypeConstants.Treasure };

            var generatedTreasure = new Treasure();
            mockTreasureGenerator.Setup(g => g.GenerateAtLevel(9266)).Returns(generatedTreasure);

            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.TreasureContainers)).Returns("container");
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.TreasureConcealment)).Returns("peek-a-boo");

            var contents = contentsGenerator.Generate(9266);
            Assert.That(contents.IsEmpty, Is.False);
            Assert.That(contents.Miscellaneous.Single(), Is.EqualTo(ContentsTypeConstants.Treasure));

            var treasure = contents.Treasures.Single();
            Assert.That(treasure.Treasure, Is.EqualTo(generatedTreasure));
            Assert.That(treasure.Container, Is.EqualTo("container"));
            Assert.That(treasure.Concealment, Is.EqualTo("peek-a-boo"));
        }
    }
}

using DnDGen.DungeonGen.Generators.AreaGenerators;
using DnDGen.DungeonGen.Generators.ContentGenerators;
using DnDGen.DungeonGen.Generators.ExitGenerators;
using DnDGen.DungeonGen.Generators.Factories;
using DnDGen.DungeonGen.Models;
using DnDGen.DungeonGen.Selectors;
using DnDGen.DungeonGen.Tables;
using DnDGen.EncounterGen.Generators;
using DnDGen.EncounterGen.Models;
using DnDGen.Infrastructure.Generators;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace DnDGen.DungeonGen.Tests.Unit.Generators.AreaGenerators
{
    [TestFixture]
    public class ChamberGeneratorTests
    {
        private AreaGenerator chamberGenerator;
        private Mock<IAreaPercentileSelector> mockAreaPercentileSelector;
        private Area selectedChamber;
        private Mock<AreaGenerator> mockSpecialAreaGenerator;
        private Mock<ExitGenerator> mockExitGenerator;
        private Mock<ContentsGenerator> mockContentsGenerator;
        private Mock<AreaGeneratorFactory> mockAreaGeneratorFactory;
        private Mock<JustInTimeFactory> mockJustInTimeFactory;
        private EncounterSpecifications specifications;

        [SetUp]
        public void Setup()
        {
            mockAreaPercentileSelector = new Mock<IAreaPercentileSelector>();
            mockSpecialAreaGenerator = new Mock<AreaGenerator>();
            mockExitGenerator = new Mock<ExitGenerator>();
            mockContentsGenerator = new Mock<ContentsGenerator>();
            mockAreaGeneratorFactory = new Mock<AreaGeneratorFactory>();
            mockJustInTimeFactory = new Mock<JustInTimeFactory>();
            chamberGenerator = new ChamberGenerator(
                mockAreaPercentileSelector.Object,
                mockAreaGeneratorFactory.Object,
                mockJustInTimeFactory.Object,
                mockContentsGenerator.Object);

            specifications = new EncounterSpecifications();
            selectedChamber = new Area();
            selectedChamber.Length = 9266;
            selectedChamber.Width = 90210;

            mockAreaGeneratorFactory.Setup(f => f.Build(AreaTypeConstants.Special)).Returns(mockSpecialAreaGenerator.Object);
            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.Chambers)).Returns(selectedChamber);
            mockContentsGenerator.Setup(g => g.Generate(It.IsAny<int>())).Returns(() => new Contents());
            mockJustInTimeFactory.Setup(f => f.Build<ExitGenerator>(AreaTypeConstants.Chamber)).Returns(mockExitGenerator.Object);
        }

        [Test]
        public void AreaTypeIsChamber()
        {
            Assert.That(chamberGenerator.AreaType, Is.EqualTo(AreaTypeConstants.Chamber));
        }

        [Test]
        public void GenerateChamber()
        {
            var chambers = chamberGenerator.Generate(42, specifications);
            Assert.That(chambers, Is.Not.Null);
            Assert.That(chambers, Is.Not.Empty);
        }

        [Test]
        public void GenerateChamberFromSelector()
        {
            var chambers = chamberGenerator.Generate(42, specifications);
            Assert.That(chambers.Single(), Is.EqualTo(selectedChamber));
        }

        [Test]
        public void GenerateChamberExits()
        {
            var firstExit = new Area();
            var secondExit = new Area();
            mockExitGenerator.Setup(g => g.Generate(42, specifications, 9266, 90210)).Returns(new[] { firstExit, secondExit });

            var chambers = chamberGenerator.Generate(42, specifications);
            Assert.That(chambers, Contains.Item(selectedChamber));
            Assert.That(chambers, Contains.Item(firstExit));
            Assert.That(chambers, Contains.Item(secondExit));
        }

        [Test]
        public void GenerateChamberContents()
        {
            specifications.Level = 600;

            var generatedContents = new Contents();
            generatedContents.Encounters = [new Encounter(), new Encounter()];
            generatedContents.Miscellaneous = ["thing 1", "thing 2"];
            generatedContents.Traps = [new Trap(), new Trap()];
            generatedContents.Treasures = [new DungeonTreasure(), new DungeonTreasure()];

            mockContentsGenerator.Setup(g => g.Generate(600)).Returns(generatedContents);

            var chambers = chamberGenerator.Generate(42, specifications);
            var contents = chambers.Single().Contents;

            Assert.That(contents.Encounters.Count(), Is.EqualTo(2));
            Assert.That(contents.Miscellaneous, Contains.Item("thing 1"));
            Assert.That(contents.Miscellaneous, Contains.Item("thing 2"));
            Assert.That(contents.Miscellaneous.Count(), Is.EqualTo(2));
            Assert.That(contents.Traps.Count(), Is.EqualTo(2));
            Assert.That(contents.Treasures.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GenerateSpecialChamber()
        {
            selectedChamber.Type = AreaTypeConstants.Special;
            var firstSpecialArea = new Area();
            var secondSpecialArea = new Area();
            var specialAreas = new[] { firstSpecialArea, secondSpecialArea };

            mockSpecialAreaGenerator.Setup(g => g.Generate(42, specifications)).Returns(specialAreas);

            var chambers = chamberGenerator.Generate(42, specifications);
            Assert.That(chambers, Is.EqualTo(specialAreas));
        }

        [Test]
        public void GenerateSpecialChamberWithContents()
        {
            specifications.Level = 600;

            selectedChamber.Type = AreaTypeConstants.Special;
            var firstSpecialArea = new Area();
            firstSpecialArea.Contents.Encounters = new[] { new Encounter() };
            firstSpecialArea.Contents.Traps = new[] { new Trap() };

            var secondSpecialArea = new Area();
            secondSpecialArea.Contents.Miscellaneous = new[] { "thing 1", "thing 2" };
            secondSpecialArea.Descriptions = new[] { "a cave" };
            secondSpecialArea.Contents.Treasures = new[] { new DungeonTreasure() };
            secondSpecialArea.Contents.Pool = new Pool();

            var firstContents = new Contents();
            firstContents.Miscellaneous = new[] { "new stuff" };
            firstContents.Encounters = new[] { new Encounter() };

            var secondContents = new Contents();
            secondContents.Miscellaneous = new[] { "other new stuff" };
            secondContents.Treasures = new[] { new DungeonTreasure() };
            secondContents.Traps = new[] { new Trap() };

            //INFO: We do the order backwards, becuase the internals will iterate backwards through the list
            mockContentsGenerator.SetupSequence(g => g.Generate(600)).Returns(secondContents).Returns(firstContents);

            var specialAreas = new[] { firstSpecialArea, secondSpecialArea };

            mockSpecialAreaGenerator.Setup(g => g.Generate(42, specifications)).Returns(specialAreas);

            var chambers = chamberGenerator.Generate(42, specifications);
            Assert.That(chambers, Is.EqualTo(specialAreas));

            var first = chambers.First();
            var last = chambers.Last();

            Assert.That(first.Contents.Encounters.Count(), Is.EqualTo(2));
            Assert.That(first.Contents.Miscellaneous.Single(), Is.EqualTo("new stuff"));
            Assert.That(first.Contents.Traps.Count(), Is.EqualTo(1));
            Assert.That(first.Contents.Treasures, Is.Empty);
            Assert.That(first.Descriptions, Is.Empty);

            Assert.That(last.Contents.Encounters, Is.Empty);
            Assert.That(last.Contents.Miscellaneous, Contains.Item("thing 1"));
            Assert.That(last.Contents.Miscellaneous, Contains.Item("thing 2"));
            Assert.That(last.Contents.Miscellaneous, Contains.Item("other new stuff"));
            Assert.That(last.Contents.Miscellaneous.Count(), Is.EqualTo(3));
            Assert.That(last.Contents.Traps.Count(), Is.EqualTo(1));
            Assert.That(last.Contents.Treasures.Count(), Is.EqualTo(2));
            Assert.That(last.Descriptions.Single(), Is.EqualTo("a cave"));
            Assert.That(last.Contents.Pool, Is.Not.Null);
            Assert.That(last.Contents.Pool, Is.EqualTo(secondSpecialArea.Contents.Pool));
        }

        [Test]
        public void GenerateSpecialChamberExits()
        {
            selectedChamber.Type = AreaTypeConstants.Special;

            var firstSpecialArea = new Area();
            firstSpecialArea.Length = 9266;
            firstSpecialArea.Width = 90210;

            var secondSpecialArea = new Area();
            secondSpecialArea.Length = 1234;
            secondSpecialArea.Width = 1337;

            var specialAreas = new[] { firstSpecialArea, secondSpecialArea };

            mockSpecialAreaGenerator.Setup(g => g.Generate(42, specifications)).Returns(specialAreas);

            var firstExit = new Area();
            var secondExit = new Area();
            mockExitGenerator.Setup(g => g.Generate(42, specifications, 9266, 90210)).Returns(new[] { firstExit, secondExit });

            var thirdExit = new Area();
            var fourthExit = new Area();
            mockExitGenerator.Setup(g => g.Generate(42, specifications, 1234, 1337)).Returns(new[] { thirdExit, fourthExit });

            var chambers = chamberGenerator.Generate(42, specifications).ToArray();
            Assert.That(chambers[0], Is.EqualTo(firstSpecialArea));
            Assert.That(chambers[1], Is.EqualTo(firstExit));
            Assert.That(chambers[2], Is.EqualTo(secondExit));
            Assert.That(chambers[3], Is.EqualTo(secondSpecialArea));
            Assert.That(chambers[4], Is.EqualTo(thirdExit));
            Assert.That(chambers[5], Is.EqualTo(fourthExit));
        }

        [Test]
        public void IfSpecialAreaTypeIsBlank_AssignChamber()
        {
            selectedChamber.Type = AreaTypeConstants.Special;
            var firstSpecialArea = new Area();
            var secondSpecialArea = new Area();
            var specialAreas = new[] { firstSpecialArea, secondSpecialArea };

            mockSpecialAreaGenerator.Setup(g => g.Generate(42, specifications)).Returns(specialAreas);

            var chambers = chamberGenerator.Generate(42, specifications);
            Assert.That(chambers, Is.EqualTo(specialAreas));
            Assert.That(chambers.First().Type, Is.EqualTo(AreaTypeConstants.Chamber));
            Assert.That(chambers.Last().Type, Is.EqualTo(AreaTypeConstants.Chamber));
        }

        [Test]
        public void IfSpecialAreaTypeIsNotBlank_DoNotAssignChamber()
        {
            selectedChamber.Type = AreaTypeConstants.Special;
            var firstSpecialArea = new Area { Type = "cave" };
            var secondSpecialArea = new Area { Type = "whatever" };
            var specialAreas = new[] { firstSpecialArea, secondSpecialArea };

            mockSpecialAreaGenerator.Setup(g => g.Generate(42, specifications)).Returns(specialAreas);

            var chambers = chamberGenerator.Generate(42, specifications);
            Assert.That(chambers, Is.EqualTo(specialAreas));
            Assert.That(chambers.First().Type, Is.EqualTo("cave"));
            Assert.That(chambers.Last().Type, Is.EqualTo("whatever"));
        }

        [Test]
        public void DetermineWhetherToAssignChamberPerSpecialArea()
        {
            selectedChamber.Type = AreaTypeConstants.Special;
            var firstSpecialArea = new Area { Type = "cave" };
            var secondSpecialArea = new Area();
            var specialAreas = new[] { firstSpecialArea, secondSpecialArea };

            mockSpecialAreaGenerator.Setup(g => g.Generate(42, specifications)).Returns(specialAreas);

            var chambers = chamberGenerator.Generate(42, specifications);
            Assert.That(chambers, Is.EqualTo(specialAreas));
            Assert.That(chambers.First().Type, Is.EqualTo("cave"));
            Assert.That(chambers.Last().Type, Is.EqualTo(AreaTypeConstants.Chamber));
        }
    }
}

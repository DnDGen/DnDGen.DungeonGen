using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.AreaGenerators;
using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using EncounterGen.Common;
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
        private Mock<AreaGenerator> mockSpecialAreaGenerator;
        private Mock<ExitGenerator> mockExitGenerator;
        private Mock<ContentsGenerator> mockContentsGenerator;

        [SetUp]
        public void Setup()
        {
            mockAreaPercentileSelector = new Mock<IAreaPercentileSelector>();
            mockSpecialAreaGenerator = new Mock<AreaGenerator>();
            mockExitGenerator = new Mock<ExitGenerator>();
            mockContentsGenerator = new Mock<ContentsGenerator>();
            chamberGenerator = new ChamberGenerator(mockAreaPercentileSelector.Object, mockSpecialAreaGenerator.Object, mockExitGenerator.Object, mockContentsGenerator.Object);

            selectedChamber = new Area();
            selectedChamber.Length = 9266;
            selectedChamber.Width = 90210;

            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.Chambers)).Returns(selectedChamber);
            mockContentsGenerator.Setup(g => g.Generate(It.IsAny<int>())).Returns(() => new Contents());
        }

        [Test]
        public void GenerateChamber()
        {
            var chambers = chamberGenerator.Generate(42, 600);
            Assert.That(chambers, Is.Not.Null);
            Assert.That(chambers, Is.Not.Empty);
        }

        [Test]
        public void GenerateChamberFromSelector()
        {
            var chambers = chamberGenerator.Generate(42, 600);
            Assert.That(chambers.Single(), Is.EqualTo(selectedChamber));
        }

        [Test]
        public void GenerateChamberExits()
        {
            var firstExit = new Area();
            var secondExit = new Area();
            mockExitGenerator.Setup(g => g.Generate(42, 600, 9266, 90210)).Returns(new[] { firstExit, secondExit });

            var chambers = chamberGenerator.Generate(42, 600);
            Assert.That(chambers, Contains.Item(selectedChamber));
            Assert.That(chambers, Contains.Item(firstExit));
            Assert.That(chambers, Contains.Item(secondExit));
        }

        [Test]
        public void GenerateChamberContents()
        {
            var generatedContents = new Contents();
            generatedContents.Encounters = new[] { new Encounter(), new Encounter() };
            generatedContents.Miscellaneous = new[] { "thing 1", "thing 2" };
            generatedContents.Traps = new[] { new Trap(), new Trap() };
            generatedContents.Treasures = new[] { new DungeonTreasure(), new DungeonTreasure() };

            mockContentsGenerator.Setup(g => g.Generate(600)).Returns(generatedContents);

            var chambers = chamberGenerator.Generate(42, 600);
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

            mockSpecialAreaGenerator.Setup(g => g.Generate(42, 600)).Returns(specialAreas);

            var chambers = chamberGenerator.Generate(42, 600);
            Assert.That(chambers, Is.EqualTo(specialAreas));
        }

        [Test]
        public void GenerateSpecialChamberWithContents()
        {
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

            mockSpecialAreaGenerator.Setup(g => g.Generate(42, 600)).Returns(specialAreas);

            var chambers = chamberGenerator.Generate(42, 600);
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

            mockSpecialAreaGenerator.Setup(g => g.Generate(42, 600)).Returns(specialAreas);

            var firstExit = new Area();
            var secondExit = new Area();
            mockExitGenerator.Setup(g => g.Generate(42, 600, 9266, 90210)).Returns(new[] { firstExit, secondExit });

            var thirdExit = new Area();
            var fourthExit = new Area();
            mockExitGenerator.Setup(g => g.Generate(42, 600, 1234, 1337)).Returns(new[] { thirdExit, fourthExit });

            var chambers = chamberGenerator.Generate(42, 600).ToArray();
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

            mockSpecialAreaGenerator.Setup(g => g.Generate(42, 600)).Returns(specialAreas);

            var chambers = chamberGenerator.Generate(42, 600);
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

            mockSpecialAreaGenerator.Setup(g => g.Generate(42, 600)).Returns(specialAreas);

            var chambers = chamberGenerator.Generate(42, 600);
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

            mockSpecialAreaGenerator.Setup(g => g.Generate(42, 600)).Returns(specialAreas);

            var chambers = chamberGenerator.Generate(42, 600);
            Assert.That(chambers, Is.EqualTo(specialAreas));
            Assert.That(chambers.First().Type, Is.EqualTo("cave"));
            Assert.That(chambers.Last().Type, Is.EqualTo(AreaTypeConstants.Chamber));
        }
    }
}

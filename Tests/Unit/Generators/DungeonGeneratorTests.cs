using DungeonGen.Common;
using DungeonGen.Generators;
using DungeonGen.Generators.Domain;
using DungeonGen.Generators.Domain.RuntimeFactories;
using DungeonGen.Selectors;
using DungeonGen.Tables;
using EncounterGen.Common;
using EncounterGen.Generators;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace DungeonGen.Tests.Unit.Generators
{
    [TestFixture]
    public class DungeonGeneratorTests
    {
        private IDungeonGenerator dungeonGenerator;
        private Mock<IAreaPercentileSelector> mockAreaPercentileSelector;
        private Mock<IAreaGeneratorFactory> mockAreaGeneratorFactory;
        private Mock<IEncounterGenerator> mockEncounterGenerator;
        private Mock<ITrapGenerator> mockTrapGenerator;
        private Mock<IPercentileSelector> mockPercentileSelector;

        [SetUp]
        public void Setup()
        {
            mockAreaPercentileSelector = new Mock<IAreaPercentileSelector>();
            mockEncounterGenerator = new Mock<IEncounterGenerator>();
            mockAreaGeneratorFactory = new Mock<IAreaGeneratorFactory>();
            mockTrapGenerator = new Mock<ITrapGenerator>();
            mockPercentileSelector = new Mock<IPercentileSelector>();
            dungeonGenerator = new DungeonGenerator(mockAreaPercentileSelector.Object, mockAreaGeneratorFactory.Object, mockEncounterGenerator.Object, mockTrapGenerator.Object, mockPercentileSelector.Object);
        }

        [Test]
        public void GenerateHallAreasFromTable()
        {
            var areaFromHall = new Area();
            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.DungeonAreaFromHall)).Returns(areaFromHall);

            var areas = dungeonGenerator.GenerateFromHall(9266);
            Assert.That(areas, Contains.Item(areaFromHall));
            Assert.That(areas.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GenerateGeneralAreaFromHall()
        {
            var areaFromHall = new Area();
            var areaReroll = new Area();

            areaReroll.Type = AreaTypeConstants.General;
            areaReroll.Contents.Miscellaneous = new[] { "contents 1", "contents 2" };

            areaFromHall.Type = "area type";
            areaFromHall.Contents.Miscellaneous = new[] { "contents 3", "contents 4" };

            mockAreaPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DungeonAreaFromHall))
                .Returns(areaReroll).Returns(areaFromHall);

            var areas = dungeonGenerator.GenerateFromHall(9266);
            var firstArea = areas.First();
            var lastArea = areas.Last();

            Assert.That(firstArea, Is.Not.EqualTo(lastArea));
            Assert.That(areas.Count(), Is.EqualTo(2));

            Assert.That(firstArea.Type, Is.EqualTo(AreaTypeConstants.General));
            Assert.That(firstArea.Contents.Miscellaneous, Contains.Item("contents 1"));
            Assert.That(firstArea.Contents.Miscellaneous, Contains.Item("contents 2"));
            Assert.That(firstArea.Contents.Miscellaneous.Count(), Is.EqualTo(2));

            Assert.That(lastArea.Type, Is.EqualTo("area type"));
            Assert.That(lastArea.Contents.Miscellaneous, Contains.Item("contents 3"));
            Assert.That(lastArea.Contents.Miscellaneous, Contains.Item("contents 4"));
            Assert.That(lastArea.Contents.Miscellaneous.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GenerateMultipleGeneralAreasFromHall()
        {
            var areaFromHall = new Area();
            var areaReroll = new Area();
            var otherAreaReroll = new Area();

            areaReroll.Type = AreaTypeConstants.General;
            areaReroll.Contents.Miscellaneous = new[] { "contents 1", "contents 2" };

            otherAreaReroll.Type = AreaTypeConstants.General;
            otherAreaReroll.Contents.Miscellaneous = new[] { "contents a", "contents b" };

            areaFromHall.Type = "area type";
            areaFromHall.Contents.Miscellaneous = new[] { "contents 3", "contents 4" };

            mockAreaPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DungeonAreaFromHall))
                .Returns(areaReroll).Returns(otherAreaReroll).Returns(areaFromHall);

            var areas = dungeonGenerator.GenerateFromHall(9266).ToList();
            Assert.That(areas.Count, Is.EqualTo(3));

            Assert.That(areas[0].Type, Is.EqualTo(AreaTypeConstants.General));
            Assert.That(areas[0].Contents.Miscellaneous, Contains.Item("contents 1"));
            Assert.That(areas[0].Contents.Miscellaneous, Contains.Item("contents 2"));
            Assert.That(areas[0].Contents.Miscellaneous.Count(), Is.EqualTo(2));

            Assert.That(areas[1].Type, Is.EqualTo(AreaTypeConstants.General));
            Assert.That(areas[1].Contents.Miscellaneous, Contains.Item("contents a"));
            Assert.That(areas[1].Contents.Miscellaneous, Contains.Item("contents b"));
            Assert.That(areas[1].Contents.Miscellaneous.Count(), Is.EqualTo(2));

            Assert.That(areas[2].Type, Is.EqualTo("area type"));
            Assert.That(areas[2].Contents.Miscellaneous, Contains.Item("contents 3"));
            Assert.That(areas[2].Contents.Miscellaneous, Contains.Item("contents 4"));
            Assert.That(areas[2].Contents.Miscellaneous.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GenerateSpecificAreaFromHall()
        {
            var areaFromHall = new Area();
            areaFromHall.Type = "area type";
            areaFromHall.Width = 1;

            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.DungeonAreaFromHall)).Returns(areaFromHall);

            var mockAreaGenerator = new Mock<AreaGenerator>();
            mockAreaGeneratorFactory.Setup(f => f.HasSpecificGenerator("area type")).Returns(true);
            mockAreaGeneratorFactory.Setup(f => f.Build("area type")).Returns(mockAreaGenerator.Object);

            var specificArea = new Area();
            var otherSpecificArea = new Area();

            mockAreaGenerator.Setup(g => g.Generate(9266)).Returns(new[] { specificArea, otherSpecificArea });

            var areas = dungeonGenerator.GenerateFromHall(9266);
            Assert.That(areas, Contains.Item(specificArea));
            Assert.That(areas, Contains.Item(otherSpecificArea));
            Assert.That(areas.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GenerateMultipleSpecificAreasFromHall()
        {
            var areaFromHall = new Area();
            areaFromHall.Type = "area type";
            areaFromHall.Width = 2;
            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.DungeonAreaFromHall)).Returns(areaFromHall);

            var mockAreaGenerator = new Mock<AreaGenerator>();
            mockAreaGeneratorFactory.Setup(f => f.HasSpecificGenerator("area type")).Returns(true);
            mockAreaGeneratorFactory.Setup(f => f.Build("area type")).Returns(mockAreaGenerator.Object);

            var specificArea = new Area();
            var otherSpecificArea = new Area();
            var thirdSpecificArea = new Area();
            var fourthSpecificArea = new Area();

            mockAreaGenerator.SetupSequence(g => g.Generate(9266))
                .Returns(new[] { specificArea, otherSpecificArea })
                .Returns(new[] { thirdSpecificArea, fourthSpecificArea });

            var areas = dungeonGenerator.GenerateFromHall(9266);
            Assert.That(areas, Contains.Item(specificArea));
            Assert.That(areas, Contains.Item(otherSpecificArea));
            Assert.That(areas, Contains.Item(thirdSpecificArea));
            Assert.That(areas, Contains.Item(fourthSpecificArea));
            Assert.That(areas.Count(), Is.EqualTo(4));
        }

        [Test]
        public void GenerateGeneralAndSpecificAreasFromHall()
        {
            var generalArea = new Area();
            generalArea.Type = AreaTypeConstants.General;
            generalArea.Contents.Miscellaneous = new[] { "contents 1", "contents 2" };

            var areaFromHall = new Area();
            areaFromHall.Type = "area type";
            areaFromHall.Width = 1;

            mockAreaPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DungeonAreaFromHall))
                .Returns(generalArea).Returns(areaFromHall);

            var mockAreaGenerator = new Mock<AreaGenerator>();
            mockAreaGeneratorFactory.Setup(f => f.HasSpecificGenerator("area type")).Returns(true);
            mockAreaGeneratorFactory.Setup(f => f.Build("area type")).Returns(mockAreaGenerator.Object);

            var specificArea = new Area();
            var otherSpecificArea = new Area();
            mockAreaGenerator.Setup(g => g.Generate(9266)).Returns(new[] { specificArea, otherSpecificArea });

            var areas = dungeonGenerator.GenerateFromHall(9266);
            Assert.That(areas, Contains.Item(generalArea));
            Assert.That(areas, Contains.Item(specificArea));
            Assert.That(areas, Contains.Item(otherSpecificArea));
            Assert.That(areas.Count(), Is.EqualTo(3));
        }

        [Test]
        public void GenerateEncountersFromHall()
        {
            var generalArea = new Area();
            generalArea.Type = AreaTypeConstants.General;
            generalArea.Contents.Miscellaneous = new[] { ContentsTypeConstants.Encounter, ContentsTypeConstants.Encounter };

            var areaFromHall = new Area();
            areaFromHall.Type = "area type";
            areaFromHall.Width = 1;

            mockAreaPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DungeonAreaFromHall))
                .Returns(generalArea).Returns(areaFromHall);

            var mockAreaGenerator = new Mock<AreaGenerator>();
            mockAreaGeneratorFactory.Setup(f => f.HasSpecificGenerator("area type")).Returns(true);
            mockAreaGeneratorFactory.Setup(f => f.Build("area type")).Returns(mockAreaGenerator.Object);

            var specificArea = new Area();
            var otherSpecificArea = new Area();

            specificArea.Contents.Miscellaneous = new[] { ContentsTypeConstants.Encounter };
            otherSpecificArea.Contents.Miscellaneous = new[] { ContentsTypeConstants.Encounter };

            mockAreaGenerator.Setup(g => g.Generate(9266)).Returns(new[] { specificArea, otherSpecificArea });
            mockEncounterGenerator.Setup(g => g.Generate(EnvironmentConstants.Dungeon, 9266)).Returns(() => new Encounter());

            var areas = dungeonGenerator.GenerateFromHall(9266);
            Assert.That(areas, Contains.Item(generalArea));
            Assert.That(areas, Contains.Item(specificArea));
            Assert.That(areas, Contains.Item(otherSpecificArea));
            Assert.That(areas.Count(), Is.EqualTo(3));

            Assert.That(generalArea.Contents.Encounters.Count(), Is.EqualTo(2));
            Assert.That(generalArea.Contents.Encounters, Is.Unique);
            Assert.That(specificArea.Contents.Encounters.Count(), Is.EqualTo(1));
            Assert.That(otherSpecificArea.Contents.Encounters.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GenerateTrapsFromHall()
        {
            var generalArea = new Area();
            generalArea.Type = AreaTypeConstants.General;
            generalArea.Contents.Miscellaneous = new[] { ContentsTypeConstants.Trap, ContentsTypeConstants.Trap };

            var areaFromHall = new Area();
            areaFromHall.Type = "area type";
            areaFromHall.Width = 1;

            mockAreaPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DungeonAreaFromHall))
                .Returns(generalArea).Returns(areaFromHall);

            var mockAreaGenerator = new Mock<AreaGenerator>();
            mockAreaGeneratorFactory.Setup(f => f.HasSpecificGenerator("area type")).Returns(true);
            mockAreaGeneratorFactory.Setup(f => f.Build("area type")).Returns(mockAreaGenerator.Object);

            var specificArea = new Area();
            var otherSpecificArea = new Area();

            specificArea.Contents.Miscellaneous = new[] { ContentsTypeConstants.Trap };
            otherSpecificArea.Contents.Miscellaneous = new[] { ContentsTypeConstants.Trap };

            mockAreaGenerator.Setup(g => g.Generate(9266)).Returns(new[] { specificArea, otherSpecificArea });
            mockTrapGenerator.Setup(g => g.Generate(9266)).Returns(() => new Trap());

            var areas = dungeonGenerator.GenerateFromHall(9266);
            Assert.That(areas, Contains.Item(generalArea));
            Assert.That(areas, Contains.Item(specificArea));
            Assert.That(areas, Contains.Item(otherSpecificArea));
            Assert.That(areas.Count(), Is.EqualTo(3));

            Assert.That(generalArea.Contents.Traps.Count(), Is.EqualTo(2));
            Assert.That(generalArea.Contents.Traps, Is.Unique);
            Assert.That(specificArea.Contents.Traps.Count(), Is.EqualTo(1));
            Assert.That(otherSpecificArea.Contents.Traps.Count(), Is.EqualTo(1));
        }

        [Test]
        public void SetDoorLocationFromHall()
        {
            var areaFromHall = new Area();
            areaFromHall.Type = "area type";
            areaFromHall.Width = 2;
            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.DungeonAreaFromHall)).Returns(areaFromHall);

            var mockAreaGenerator = new Mock<AreaGenerator>();
            mockAreaGeneratorFactory.Setup(f => f.HasSpecificGenerator("area type")).Returns(true);
            mockAreaGeneratorFactory.Setup(f => f.Build("area type")).Returns(mockAreaGenerator.Object);

            var door = new Area { Type = AreaTypeConstants.Door, Descriptions = new[] { "description" } };
            var otherDoor = new Area { Type = AreaTypeConstants.Door, Descriptions = new[] { "other description", "locked" } };

            mockAreaGenerator.SetupSequence(g => g.Generate(9266)).Returns(new[] { door }).Returns(new[] { otherDoor });
            mockPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DoorLocations)).Returns("below").Returns("on the ceiling");

            var areas = dungeonGenerator.GenerateFromHall(9266);
            Assert.That(areas, Contains.Item(door));
            Assert.That(areas, Contains.Item(otherDoor));
            Assert.That(areas.Count(), Is.EqualTo(2));

            Assert.That(door.Descriptions, Contains.Item("below"));
            Assert.That(door.Descriptions, Contains.Item("description"));
            Assert.That(door.Descriptions.Count(), Is.EqualTo(2));

            Assert.That(otherDoor.Descriptions, Contains.Item("on the ceiling"));
            Assert.That(otherDoor.Descriptions, Contains.Item("other description"));
            Assert.That(otherDoor.Descriptions, Contains.Item("locked"));
            Assert.That(otherDoor.Descriptions.Count(), Is.EqualTo(3));
        }

        [Test]
        public void GenerateDoorAreasFromTable()
        {
            var areaFromDoor = new Area();
            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.DungeonAreaFromDoor)).Returns(areaFromDoor);

            var areas = dungeonGenerator.GenerateFromDoor(9266);
            Assert.That(areas, Contains.Item(areaFromDoor));
            Assert.That(areas.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GenerateGeneralAreaFromDoor()
        {
            var areaFromDoor = new Area();
            var areaReroll = new Area();

            areaReroll.Type = AreaTypeConstants.General;
            areaReroll.Contents.Miscellaneous = new[] { "contents 1", "contents 2" };

            areaFromDoor.Type = "area type";
            areaFromDoor.Contents.Miscellaneous = new[] { "contents 3", "contents 4" };

            mockAreaPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DungeonAreaFromDoor))
                .Returns(areaReroll).Returns(areaFromDoor);

            var areas = dungeonGenerator.GenerateFromDoor(9266);
            var firstArea = areas.First();
            var lastArea = areas.Last();

            Assert.That(firstArea, Is.Not.EqualTo(lastArea));
            Assert.That(areas.Count(), Is.EqualTo(2));

            Assert.That(firstArea.Type, Is.EqualTo(AreaTypeConstants.General));
            Assert.That(firstArea.Contents.Miscellaneous, Contains.Item("contents 1"));
            Assert.That(firstArea.Contents.Miscellaneous, Contains.Item("contents 2"));
            Assert.That(firstArea.Contents.Miscellaneous.Count(), Is.EqualTo(2));

            Assert.That(lastArea.Type, Is.EqualTo("area type"));
            Assert.That(lastArea.Contents.Miscellaneous, Contains.Item("contents 3"));
            Assert.That(lastArea.Contents.Miscellaneous, Contains.Item("contents 4"));
            Assert.That(lastArea.Contents.Miscellaneous.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GenerateMultipleGeneralAreasFromDoor()
        {
            var areaFromDoor = new Area();
            var areaReroll = new Area();
            var otherAreaReroll = new Area();

            areaReroll.Type = AreaTypeConstants.General;
            areaReroll.Contents.Miscellaneous = new[] { "contents 1", "contents 2" };

            otherAreaReroll.Type = AreaTypeConstants.General;
            otherAreaReroll.Contents.Miscellaneous = new[] { "contents a", "contents b" };

            areaFromDoor.Type = "area type";
            areaFromDoor.Contents.Miscellaneous = new[] { "contents 3", "contents 4" };

            mockAreaPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DungeonAreaFromDoor))
                .Returns(areaReroll).Returns(otherAreaReroll).Returns(areaFromDoor);

            var areas = dungeonGenerator.GenerateFromDoor(9266).ToList();
            Assert.That(areas.Count, Is.EqualTo(3));

            Assert.That(areas[0].Type, Is.EqualTo(AreaTypeConstants.General));
            Assert.That(areas[0].Contents.Miscellaneous, Contains.Item("contents 1"));
            Assert.That(areas[0].Contents.Miscellaneous, Contains.Item("contents 2"));
            Assert.That(areas[0].Contents.Miscellaneous.Count(), Is.EqualTo(2));

            Assert.That(areas[1].Type, Is.EqualTo(AreaTypeConstants.General));
            Assert.That(areas[1].Contents.Miscellaneous, Contains.Item("contents a"));
            Assert.That(areas[1].Contents.Miscellaneous, Contains.Item("contents b"));
            Assert.That(areas[1].Contents.Miscellaneous.Count(), Is.EqualTo(2));

            Assert.That(areas[2].Type, Is.EqualTo("area type"));
            Assert.That(areas[2].Contents.Miscellaneous, Contains.Item("contents 3"));
            Assert.That(areas[2].Contents.Miscellaneous, Contains.Item("contents 4"));
            Assert.That(areas[2].Contents.Miscellaneous.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GenerateSpecificAreaFromDoor()
        {
            var areaFromDoor = new Area();
            areaFromDoor.Type = "area type";
            areaFromDoor.Width = 1;

            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.DungeonAreaFromDoor)).Returns(areaFromDoor);

            var mockAreaGenerator = new Mock<AreaGenerator>();
            mockAreaGeneratorFactory.Setup(f => f.HasSpecificGenerator("area type")).Returns(true);
            mockAreaGeneratorFactory.Setup(f => f.Build("area type")).Returns(mockAreaGenerator.Object);

            var specificArea = new Area();
            var otherSpecificArea = new Area();
            mockAreaGenerator.Setup(g => g.Generate(9266)).Returns(new[] { specificArea, otherSpecificArea });

            var areas = dungeonGenerator.GenerateFromDoor(9266);
            Assert.That(areas, Contains.Item(specificArea));
            Assert.That(areas, Contains.Item(otherSpecificArea));
            Assert.That(areas.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GenerateMultipleSpecificAreasFromDoor()
        {
            var areaFromDoor = new Area();
            areaFromDoor.Type = "area type";
            areaFromDoor.Width = 2;
            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.DungeonAreaFromDoor)).Returns(areaFromDoor);

            var mockAreaGenerator = new Mock<AreaGenerator>();
            mockAreaGeneratorFactory.Setup(f => f.HasSpecificGenerator("area type")).Returns(true);
            mockAreaGeneratorFactory.Setup(f => f.Build("area type")).Returns(mockAreaGenerator.Object);

            var specificArea = new Area();
            var otherSpecificArea = new Area();
            var thirdSpecificArea = new Area();
            var fourthSpecificArea = new Area();

            mockAreaGenerator.SetupSequence(g => g.Generate(9266))
                .Returns(new[] { specificArea, otherSpecificArea })
                .Returns(new[] { thirdSpecificArea, fourthSpecificArea });

            var areas = dungeonGenerator.GenerateFromDoor(9266);
            Assert.That(areas, Contains.Item(specificArea));
            Assert.That(areas, Contains.Item(otherSpecificArea));
            Assert.That(areas, Contains.Item(thirdSpecificArea));
            Assert.That(areas, Contains.Item(fourthSpecificArea));
            Assert.That(areas.Count(), Is.EqualTo(4));
        }

        [Test]
        public void GenerateGeneralAndSpecificAreasFromDoor()
        {
            var generalArea = new Area();
            generalArea.Type = AreaTypeConstants.General;
            generalArea.Contents.Miscellaneous = new[] { "contents 1", "contents 2" };

            var areaFromDoor = new Area();
            areaFromDoor.Type = "area type";
            areaFromDoor.Width = 1;

            mockAreaPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DungeonAreaFromDoor))
                .Returns(generalArea).Returns(areaFromDoor);

            var mockAreaGenerator = new Mock<AreaGenerator>();
            mockAreaGeneratorFactory.Setup(f => f.HasSpecificGenerator("area type")).Returns(true);
            mockAreaGeneratorFactory.Setup(f => f.Build("area type")).Returns(mockAreaGenerator.Object);

            var specificArea = new Area();
            var otherSpecificArea = new Area();
            mockAreaGenerator.Setup(g => g.Generate(9266)).Returns(new[] { specificArea, otherSpecificArea });

            var areas = dungeonGenerator.GenerateFromDoor(9266);
            Assert.That(areas, Contains.Item(generalArea));
            Assert.That(areas, Contains.Item(specificArea));
            Assert.That(areas, Contains.Item(otherSpecificArea));
            Assert.That(areas.Count(), Is.EqualTo(3));
        }

        [Test]
        public void GenerateEncountersFromDoor()
        {
            var generalArea = new Area();
            generalArea.Type = AreaTypeConstants.General;
            generalArea.Contents.Miscellaneous = new[] { ContentsTypeConstants.Encounter, ContentsTypeConstants.Encounter };

            var areaFromDoor = new Area();
            areaFromDoor.Type = "area type";
            areaFromDoor.Width = 1;

            mockAreaPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DungeonAreaFromDoor))
                .Returns(generalArea).Returns(areaFromDoor);

            var mockAreaGenerator = new Mock<AreaGenerator>();
            mockAreaGeneratorFactory.Setup(f => f.HasSpecificGenerator("area type")).Returns(true);
            mockAreaGeneratorFactory.Setup(f => f.Build("area type")).Returns(mockAreaGenerator.Object);

            var specificArea = new Area();
            var otherSpecificArea = new Area();

            specificArea.Contents.Miscellaneous = new[] { ContentsTypeConstants.Encounter };
            otherSpecificArea.Contents.Miscellaneous = new[] { ContentsTypeConstants.Encounter };

            mockAreaGenerator.Setup(g => g.Generate(9266)).Returns(new[] { specificArea, otherSpecificArea });
            mockEncounterGenerator.Setup(g => g.Generate(EnvironmentConstants.Dungeon, 9266)).Returns(() => new Encounter());

            var areas = dungeonGenerator.GenerateFromDoor(9266);
            Assert.That(areas, Contains.Item(generalArea));
            Assert.That(areas, Contains.Item(specificArea));
            Assert.That(areas, Contains.Item(otherSpecificArea));
            Assert.That(areas.Count(), Is.EqualTo(3));

            Assert.That(generalArea.Contents.Encounters.Count(), Is.EqualTo(2));
            Assert.That(generalArea.Contents.Encounters, Is.Unique);
            Assert.That(specificArea.Contents.Encounters.Count(), Is.EqualTo(1));
            Assert.That(otherSpecificArea.Contents.Encounters.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GenerateTrapsFromDoor()
        {
            var generalArea = new Area();
            generalArea.Type = AreaTypeConstants.General;
            generalArea.Contents.Miscellaneous = new[] { ContentsTypeConstants.Trap, ContentsTypeConstants.Trap };

            var areaFromDoor = new Area();
            areaFromDoor.Type = "area type";
            areaFromDoor.Width = 1;

            mockAreaPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DungeonAreaFromDoor))
                .Returns(generalArea).Returns(areaFromDoor);

            var mockAreaGenerator = new Mock<AreaGenerator>();
            mockAreaGeneratorFactory.Setup(f => f.HasSpecificGenerator("area type")).Returns(true);
            mockAreaGeneratorFactory.Setup(f => f.Build("area type")).Returns(mockAreaGenerator.Object);

            var specificArea = new Area();
            var otherSpecificArea = new Area();

            specificArea.Contents.Miscellaneous = new[] { ContentsTypeConstants.Trap };
            otherSpecificArea.Contents.Miscellaneous = new[] { ContentsTypeConstants.Trap };

            mockAreaGenerator.Setup(g => g.Generate(9266)).Returns(new[] { specificArea, otherSpecificArea });
            mockTrapGenerator.Setup(g => g.Generate(9266)).Returns(() => new Trap());

            var areas = dungeonGenerator.GenerateFromDoor(9266);
            Assert.That(areas, Contains.Item(generalArea));
            Assert.That(areas, Contains.Item(specificArea));
            Assert.That(areas, Contains.Item(otherSpecificArea));
            Assert.That(areas.Count(), Is.EqualTo(3));

            Assert.That(generalArea.Contents.Traps.Count(), Is.EqualTo(2));
            Assert.That(generalArea.Contents.Traps, Is.Unique);
            Assert.That(specificArea.Contents.Traps.Count(), Is.EqualTo(1));
            Assert.That(otherSpecificArea.Contents.Traps.Count(), Is.EqualTo(1));
        }
    }
}

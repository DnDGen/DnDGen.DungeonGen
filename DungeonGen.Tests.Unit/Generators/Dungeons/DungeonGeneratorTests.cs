using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.AreaGenerators;
using DungeonGen.Domain.Generators.Dungeons;
using DungeonGen.Domain.Generators.Factories;
using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using EncounterGen.Common;
using EncounterGen.Generators;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace DungeonGen.Tests.Unit.Generators.Dungeons
{
    [TestFixture]
    public class DungeonGeneratorTests
    {
        private IDungeonGenerator dungeonGenerator;
        private Mock<IAreaPercentileSelector> mockAreaPercentileSelector;
        private Mock<AreaGeneratorFactory> mockAreaGeneratorFactory;
        private Mock<IEncounterGenerator> mockEncounterGenerator;
        private Mock<ITrapGenerator> mockTrapGenerator;
        private Mock<IPercentileSelector> mockPercentileSelector;
        private Mock<AreaGenerator> mockHallGenerator;
        private Mock<JustInTimeFactory> mockJustInTimeFactory;
        private EncounterSpecifications specifications;

        [SetUp]
        public void Setup()
        {
            mockAreaPercentileSelector = new Mock<IAreaPercentileSelector>();
            mockEncounterGenerator = new Mock<IEncounterGenerator>();
            mockAreaGeneratorFactory = new Mock<AreaGeneratorFactory>();
            mockTrapGenerator = new Mock<ITrapGenerator>();
            mockPercentileSelector = new Mock<IPercentileSelector>();
            mockHallGenerator = new Mock<AreaGenerator>();
            mockJustInTimeFactory = new Mock<JustInTimeFactory>();
            dungeonGenerator = new DungeonGenerator(mockAreaPercentileSelector.Object, mockAreaGeneratorFactory.Object, mockJustInTimeFactory.Object, mockTrapGenerator.Object, mockPercentileSelector.Object);

            specifications = new EncounterSpecifications();
            specifications.Environment = "environment";
            specifications.Level = 90210;
            specifications.Temperature = "temperature";
            specifications.TimeOfDay = "time of day";

            mockAreaGeneratorFactory.Setup(f => f.Build(AreaTypeConstants.Hall)).Returns(mockHallGenerator.Object);
            mockJustInTimeFactory.Setup(f => f.Build<IEncounterGenerator>()).Returns(mockEncounterGenerator.Object);
        }

        [Test]
        public void GenerateHallAreasFromTable()
        {
            var areaFromHall = new Area();
            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.DungeonAreaFromHall)).Returns(areaFromHall);

            var areas = dungeonGenerator.GenerateFromHall(9266, specifications);
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

            var areas = dungeonGenerator.GenerateFromHall(9266, specifications);
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

            var areas = dungeonGenerator.GenerateFromHall(9266, specifications).ToList();
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

            mockAreaGenerator.Setup(g => g.Generate(9266, specifications)).Returns(new[] { specificArea, otherSpecificArea });

            var areas = dungeonGenerator.GenerateFromHall(9266, specifications);
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

            mockAreaGenerator.SetupSequence(g => g.Generate(9266, specifications))
                .Returns(new[] { specificArea, otherSpecificArea })
                .Returns(new[] { thirdSpecificArea, fourthSpecificArea });

            var areas = dungeonGenerator.GenerateFromHall(9266, specifications);
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
            mockAreaGenerator.Setup(g => g.Generate(9266, specifications)).Returns(new[] { specificArea, otherSpecificArea });

            var areas = dungeonGenerator.GenerateFromHall(9266, specifications);
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

            mockAreaGenerator.Setup(g => g.Generate(9266, specifications)).Returns(new[] { specificArea, otherSpecificArea });
            mockEncounterGenerator.Setup(g => g.Generate(
                It.Is<EncounterSpecifications>(s =>
                   !s.AllowAquatic
                   && s.Environment == specifications.Environment
                   && s.Level == specifications.Level
                   && s.Temperature == specifications.Temperature
                   && s.TimeOfDay == specifications.TimeOfDay
                ))).Returns(() => new Encounter());

            var areas = dungeonGenerator.GenerateFromHall(9266, specifications);
            Assert.That(areas, Contains.Item(generalArea));
            Assert.That(areas, Contains.Item(specificArea));
            Assert.That(areas, Contains.Item(otherSpecificArea));
            Assert.That(areas.Count(), Is.EqualTo(3));

            Assert.That(generalArea.Contents.Encounters.Count(), Is.EqualTo(2));
            Assert.That(generalArea.Contents.Encounters, Is.Unique);
            Assert.That(specificArea.Contents.Encounters.Count(), Is.EqualTo(1));
            Assert.That(otherSpecificArea.Contents.Encounters.Count(), Is.EqualTo(1));
        }

        [TestCase(ContentsConstants.Chasm, false)]
        [TestCase(ContentsConstants.Gallery, false)]
        [TestCase(ContentsConstants.GalleryStairs_Beginning, false)]
        [TestCase(ContentsConstants.GalleryStairs_End, false)]
        [TestCase(ContentsConstants.LikesAlignment, false)]
        [TestCase(ContentsConstants.MagicPool, true)]
        [TestCase(ContentsConstants.River, true)]
        [TestCase(ContentsConstants.Stream, true)]
        [TestCase(ContentsConstants.TeleportationPool, true)]
        [TestCase(ContentsTypeConstants.Encounter, false)]
        [TestCase(ContentsTypeConstants.Lake, true)]
        [TestCase(ContentsTypeConstants.Pool, true)]
        [TestCase(ContentsTypeConstants.Trap, false)]
        [TestCase(ContentsTypeConstants.Treasure, false)]
        public void EncountersFromHallIncludeAquatic(string content, bool hasAquatic)
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

            specificArea.Contents.Miscellaneous = new[] { ContentsTypeConstants.Encounter, content };
            otherSpecificArea.Contents.Miscellaneous = new[] { ContentsTypeConstants.Encounter, content };

            mockAreaGenerator.Setup(g => g.Generate(9266, specifications)).Returns(new[] { specificArea, otherSpecificArea });
            mockEncounterGenerator.Setup(g => g.Generate(
                It.Is<EncounterSpecifications>(s =>
                   s.AllowAquatic == hasAquatic
                   && s.Environment == specifications.Environment
                   && s.Level == specifications.Level
                   && s.Temperature == specifications.Temperature
                   && s.TimeOfDay == specifications.TimeOfDay
                ))).Returns(() => new Encounter());

            var generalEncounter = new Encounter();
            mockEncounterGenerator.Setup(g => g.Generate(
                It.Is<EncounterSpecifications>(s =>
                   !s.AllowAquatic
                   && s.Environment == specifications.Environment
                   && s.Level == specifications.Level
                   && s.Temperature == specifications.Temperature
                   && s.TimeOfDay == specifications.TimeOfDay
                ))).Returns(generalEncounter);

            var areas = dungeonGenerator.GenerateFromHall(9266, specifications);
            Assert.That(areas, Contains.Item(generalArea));
            Assert.That(areas, Contains.Item(specificArea));
            Assert.That(areas, Contains.Item(otherSpecificArea));
            Assert.That(areas.Count(), Is.EqualTo(3));

            Assert.That(generalArea.Contents.Encounters, Is.Not.Empty);
            Assert.That(generalArea.Contents.Encounters, Is.Not.Null);
            Assert.That(generalArea.Contents.Encounters, Is.All.EqualTo(generalEncounter));
            Assert.That(specificArea.Contents.Encounters, Is.Not.Empty);
            Assert.That(specificArea.Contents.Encounters, Is.Not.Null);
            Assert.That(otherSpecificArea.Contents.Encounters, Is.Not.Empty);
            Assert.That(otherSpecificArea.Contents.Encounters, Is.Not.Null);

            if (content != ContentsTypeConstants.Encounter)
            {
                Assert.That(specificArea.Contents.Encounters, Is.Unique);
                Assert.That(otherSpecificArea.Contents.Encounters, Is.Unique);
            }
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

            mockAreaGenerator.Setup(g => g.Generate(9266, specifications)).Returns(new[] { specificArea, otherSpecificArea });
            mockTrapGenerator.Setup(g => g.Generate(90210)).Returns(() => new Trap());

            var areas = dungeonGenerator.GenerateFromHall(9266, specifications);
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

            mockAreaGenerator.SetupSequence(g => g.Generate(9266, specifications)).Returns(new[] { door }).Returns(new[] { otherDoor });
            mockPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DoorLocations)).Returns("below").Returns("on the ceiling");

            var areas = dungeonGenerator.GenerateFromHall(9266, specifications);
            Assert.That(areas, Contains.Item(door));
            Assert.That(areas, Contains.Item(otherDoor));

            Assert.That(door.Descriptions, Contains.Item("below"));
            Assert.That(door.Descriptions, Contains.Item("description"));
            Assert.That(door.Descriptions.Count(), Is.EqualTo(2));

            Assert.That(otherDoor.Descriptions, Contains.Item("on the ceiling"));
            Assert.That(otherDoor.Descriptions, Contains.Item("other description"));
            Assert.That(otherDoor.Descriptions, Contains.Item("locked"));
            Assert.That(otherDoor.Descriptions.Count(), Is.EqualTo(3));
        }

        [Test]
        public void DoorsToSideAlsoHaveHallContinuing()
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

            mockAreaGenerator.SetupSequence(g => g.Generate(9266, specifications)).Returns(new[] { door }).Returns(new[] { otherDoor });
            mockPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DoorLocations)).Returns("below").Returns("on the ceiling");

            var areas = dungeonGenerator.GenerateFromHall(9266, specifications);
            Assert.That(areas, Contains.Item(door));
            Assert.That(areas, Contains.Item(otherDoor));
            Assert.That(areas.Count(), Is.EqualTo(3));

            Assert.That(door.Descriptions, Contains.Item("below"));
            Assert.That(door.Descriptions, Contains.Item("description"));
            Assert.That(door.Descriptions.Count(), Is.EqualTo(2));

            Assert.That(otherDoor.Descriptions, Contains.Item("on the ceiling"));
            Assert.That(otherDoor.Descriptions, Contains.Item("other description"));
            Assert.That(otherDoor.Descriptions, Contains.Item("locked"));
            Assert.That(otherDoor.Descriptions.Count(), Is.EqualTo(3));

            var last = areas.Last();
            Assert.That(last, Is.Not.EqualTo(door));
            Assert.That(last, Is.Not.EqualTo(otherDoor));
            Assert.That(last.Type, Is.EqualTo(AreaTypeConstants.Hall));
            Assert.That(last.Width, Is.EqualTo(0));
            Assert.That(last.Length, Is.EqualTo(30));
            Assert.That(last.Contents.IsEmpty, Is.True);
            Assert.That(last.Descriptions, Is.Empty);
        }

        [Test]
        public void DoorStraightAheadPreventsHallContinuing()
        {
            var areaFromHall = new Area();
            areaFromHall.Type = "area type";
            areaFromHall.Width = 2;
            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.DungeonAreaFromHall)).Returns(areaFromHall);

            var mockAreaGenerator = new Mock<AreaGenerator>();
            mockAreaGeneratorFactory.Setup(f => f.HasSpecificGenerator("area type")).Returns(true);
            mockAreaGeneratorFactory.Setup(f => f.Build("area type")).Returns(mockAreaGenerator.Object);

            var door = new Area { Type = AreaTypeConstants.Door, Descriptions = new[] { "description" } };
            var otherDoor = new Area
            {
                Type = AreaTypeConstants.Door,
                Descriptions = new[] { "other description", "locked" }
            };

            mockAreaGenerator.SetupSequence(g => g.Generate(9266, specifications)).Returns(new[] { door }).Returns(new[] { otherDoor });
            mockPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DoorLocations)).Returns("below").Returns(DescriptionConstants.StraightAhead);

            var areas = dungeonGenerator.GenerateFromHall(9266, specifications);
            Assert.That(areas, Contains.Item(door));
            Assert.That(areas, Contains.Item(otherDoor));
            Assert.That(areas.Count(), Is.EqualTo(2));

            Assert.That(door.Descriptions, Contains.Item("below"));
            Assert.That(door.Descriptions, Contains.Item("description"));
            Assert.That(door.Descriptions.Count(), Is.EqualTo(2));

            Assert.That(otherDoor.Descriptions, Contains.Item("other description"));
            Assert.That(otherDoor.Descriptions, Contains.Item("locked"));
            Assert.That(otherDoor.Descriptions, Contains.Item(DescriptionConstants.StraightAhead));
            Assert.That(otherDoor.Descriptions.Count(), Is.EqualTo(3));
        }

        [Test]
        public void ContinuingHallHasSameWidth()
        {
            var areaFromHall = new Area();
            areaFromHall.Type = AreaTypeConstants.Hall;
            areaFromHall.Width = 42;
            areaFromHall.Length = 600;

            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.DungeonAreaFromHall)).Returns(areaFromHall);

            var area = dungeonGenerator.GenerateFromHall(9266, specifications).Single();
            Assert.That(area, Is.EqualTo(areaFromHall));
            Assert.That(area.Type, Is.EqualTo(AreaTypeConstants.Hall));
            Assert.That(area.Length, Is.EqualTo(600));
            Assert.That(area.Width, Is.EqualTo(0));
        }

        [Test]
        public void HallsWithGeneralAreasContinueWithSameWidth()
        {
            var generalArea = new Area();
            generalArea.Type = AreaTypeConstants.General;
            generalArea.Contents.Miscellaneous = new[] { "contents 1", "contents 2" };

            var areaFromHall = new Area();
            areaFromHall.Type = AreaTypeConstants.Hall;
            areaFromHall.Width = 42;
            areaFromHall.Length = 600;

            mockAreaPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DungeonAreaFromHall))
                .Returns(generalArea).Returns(areaFromHall);

            var areas = dungeonGenerator.GenerateFromHall(9266, specifications);
            Assert.That(areas, Contains.Item(generalArea));
            Assert.That(areas, Contains.Item(areaFromHall));
            Assert.That(areas.Count(), Is.EqualTo(2));

            var last = areas.Last();
            Assert.That(last, Is.EqualTo(areaFromHall));
            Assert.That(last.Width, Is.EqualTo(0));
            Assert.That(last.Length, Is.EqualTo(600));
        }

        [Test]
        public void NewHallsHaveNewWidth()
        {
            var areaFromHall = new Area();
            areaFromHall.Type = "area type";
            areaFromHall.Width = 2;
            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.DungeonAreaFromHall)).Returns(areaFromHall);

            var mockAreaGenerator = new Mock<AreaGenerator>();
            mockAreaGeneratorFactory.Setup(f => f.HasSpecificGenerator("area type")).Returns(true);
            mockAreaGeneratorFactory.Setup(f => f.Build("area type")).Returns(mockAreaGenerator.Object);

            var specificArea = new Area { Width = 42 };
            var otherSpecificArea = new Area { Type = AreaTypeConstants.Hall };
            var thirdSpecificArea = new Area { Type = AreaTypeConstants.Hall, Width = 600 };

            mockAreaGenerator.SetupSequence(g => g.Generate(9266, specifications))
                .Returns(new[] { specificArea, otherSpecificArea })
                .Returns(new[] { thirdSpecificArea });

            var areas = dungeonGenerator.GenerateFromHall(9266, specifications);
            Assert.That(areas, Contains.Item(specificArea));
            Assert.That(areas, Contains.Item(otherSpecificArea));
            Assert.That(areas, Contains.Item(thirdSpecificArea));
            Assert.That(areas.Count(), Is.EqualTo(3));

            Assert.That(specificArea.Width, Is.EqualTo(42));
            Assert.That(otherSpecificArea.Width, Is.EqualTo(0));
            Assert.That(thirdSpecificArea.Width, Is.EqualTo(600));
        }

        [Test]
        public void DoNotApplyDoorLocationToChamberDoors()
        {
            var areaFromHall = new Area();
            areaFromHall.Type = "area type";
            areaFromHall.Width = 1;

            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.DungeonAreaFromHall)).Returns(areaFromHall);

            var mockAreaGenerator = new Mock<AreaGenerator>();
            mockAreaGeneratorFactory.Setup(f => f.HasSpecificGenerator("area type")).Returns(true);
            mockAreaGeneratorFactory.Setup(f => f.Build("area type")).Returns(mockAreaGenerator.Object);

            var specificArea = new Area();
            var otherSpecificArea = new Area { Type = AreaTypeConstants.Door };
            otherSpecificArea.Descriptions = new[] { "strong", "wood" };

            mockAreaGenerator.Setup(g => g.Generate(9266, specifications)).Returns(new[] { specificArea, otherSpecificArea });

            var areas = dungeonGenerator.GenerateFromHall(9266, specifications);
            Assert.That(areas, Contains.Item(specificArea));
            Assert.That(areas, Contains.Item(otherSpecificArea));
            Assert.That(areas.Count(), Is.EqualTo(2));

            var last = areas.Last();
            Assert.That(last, Is.EqualTo(otherSpecificArea));
            Assert.That(otherSpecificArea.Descriptions, Contains.Item("strong"));
            Assert.That(otherSpecificArea.Descriptions, Contains.Item("wood"));
            Assert.That(otherSpecificArea.Descriptions.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GenerateDoorAreasFromTable()
        {
            var areaFromDoor = new Area();
            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.DungeonAreaFromDoor)).Returns(areaFromDoor);

            var areas = dungeonGenerator.GenerateFromDoor(9266, specifications);
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

            var areas = dungeonGenerator.GenerateFromDoor(9266, specifications);
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

            var areas = dungeonGenerator.GenerateFromDoor(9266, specifications).ToList();
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
            mockAreaGenerator.Setup(g => g.Generate(9266, specifications)).Returns(new[] { specificArea, otherSpecificArea });

            var areas = dungeonGenerator.GenerateFromDoor(9266, specifications);
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

            mockAreaGenerator.SetupSequence(g => g.Generate(9266, specifications))
                .Returns(new[] { specificArea, otherSpecificArea })
                .Returns(new[] { thirdSpecificArea, fourthSpecificArea });

            var areas = dungeonGenerator.GenerateFromDoor(9266, specifications);
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
            mockAreaGenerator.Setup(g => g.Generate(9266, specifications)).Returns(new[] { specificArea, otherSpecificArea });

            var areas = dungeonGenerator.GenerateFromDoor(9266, specifications);
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

            mockAreaGenerator.Setup(g => g.Generate(9266, specifications)).Returns(new[] { specificArea, otherSpecificArea });
            mockEncounterGenerator.Setup(g => g.Generate(
                It.Is<EncounterSpecifications>(s =>
                   !s.AllowAquatic
                   && s.Environment == specifications.Environment
                   && s.Level == specifications.Level
                   && s.Temperature == specifications.Temperature
                   && s.TimeOfDay == specifications.TimeOfDay
                ))).Returns(() => new Encounter());

            var areas = dungeonGenerator.GenerateFromDoor(9266, specifications);
            Assert.That(areas, Contains.Item(generalArea));
            Assert.That(areas, Contains.Item(specificArea));
            Assert.That(areas, Contains.Item(otherSpecificArea));
            Assert.That(areas.Count(), Is.EqualTo(3));

            Assert.That(generalArea.Contents.Encounters.Count(), Is.EqualTo(2));
            Assert.That(generalArea.Contents.Encounters, Is.Unique);
            Assert.That(specificArea.Contents.Encounters.Count(), Is.EqualTo(1));
            Assert.That(otherSpecificArea.Contents.Encounters.Count(), Is.EqualTo(1));
        }

        [TestCase(ContentsConstants.Chasm, false)]
        [TestCase(ContentsConstants.Gallery, false)]
        [TestCase(ContentsConstants.GalleryStairs_Beginning, false)]
        [TestCase(ContentsConstants.GalleryStairs_End, false)]
        [TestCase(ContentsConstants.LikesAlignment, false)]
        [TestCase(ContentsConstants.MagicPool, true)]
        [TestCase(ContentsConstants.River, true)]
        [TestCase(ContentsConstants.Stream, true)]
        [TestCase(ContentsConstants.TeleportationPool, true)]
        [TestCase(ContentsTypeConstants.Encounter, false)]
        [TestCase(ContentsTypeConstants.Lake, true)]
        [TestCase(ContentsTypeConstants.Pool, true)]
        [TestCase(ContentsTypeConstants.Trap, false)]
        [TestCase(ContentsTypeConstants.Treasure, false)]
        public void EncountersFromDoorIncludeAquatic(string content, bool hasAquatic)
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

            specificArea.Contents.Miscellaneous = new[] { ContentsTypeConstants.Encounter, content };
            otherSpecificArea.Contents.Miscellaneous = new[] { ContentsTypeConstants.Encounter, content };

            mockAreaGenerator.Setup(g => g.Generate(9266, specifications)).Returns(new[] { specificArea, otherSpecificArea });
            mockEncounterGenerator.Setup(g => g.Generate(
                It.Is<EncounterSpecifications>(s =>
                   s.AllowAquatic == hasAquatic
                   && s.Environment == specifications.Environment
                   && s.Level == specifications.Level
                   && s.Temperature == specifications.Temperature
                   && s.TimeOfDay == specifications.TimeOfDay
                ))).Returns(() => new Encounter());

            var generalEncounter = new Encounter();
            mockEncounterGenerator.Setup(g => g.Generate(
                It.Is<EncounterSpecifications>(s =>
                   !s.AllowAquatic
                   && s.Environment == specifications.Environment
                   && s.Level == specifications.Level
                   && s.Temperature == specifications.Temperature
                   && s.TimeOfDay == specifications.TimeOfDay
                ))).Returns(generalEncounter);

            var areas = dungeonGenerator.GenerateFromDoor(9266, specifications);
            Assert.That(areas, Contains.Item(generalArea));
            Assert.That(areas, Contains.Item(specificArea));
            Assert.That(areas, Contains.Item(otherSpecificArea));
            Assert.That(areas.Count(), Is.EqualTo(3));

            Assert.That(generalArea.Contents.Encounters, Is.Not.Empty);
            Assert.That(generalArea.Contents.Encounters, Is.Not.Null);
            Assert.That(generalArea.Contents.Encounters, Is.All.EqualTo(generalEncounter));
            Assert.That(specificArea.Contents.Encounters, Is.Not.Empty);
            Assert.That(specificArea.Contents.Encounters, Is.Not.Null);
            Assert.That(otherSpecificArea.Contents.Encounters, Is.Not.Empty);
            Assert.That(otherSpecificArea.Contents.Encounters, Is.Not.Null);

            if (content != ContentsTypeConstants.Encounter)
            {
                Assert.That(specificArea.Contents.Encounters, Is.Unique);
                Assert.That(otherSpecificArea.Contents.Encounters, Is.Unique);
            }
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

            mockAreaGenerator.Setup(g => g.Generate(9266, specifications)).Returns(new[] { specificArea, otherSpecificArea });
            mockTrapGenerator.Setup(g => g.Generate(90210)).Returns(() => new Trap());

            var areas = dungeonGenerator.GenerateFromDoor(9266, specifications);
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
        public void DoNotApplyDoorLocationToRoomDoors()
        {
            var areaFromHall = new Area();
            areaFromHall.Type = "area type";
            areaFromHall.Width = 1;

            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.DungeonAreaFromDoor)).Returns(areaFromHall);

            var mockAreaGenerator = new Mock<AreaGenerator>();
            mockAreaGeneratorFactory.Setup(f => f.HasSpecificGenerator("area type")).Returns(true);
            mockAreaGeneratorFactory.Setup(f => f.Build("area type")).Returns(mockAreaGenerator.Object);

            var specificArea = new Area();
            var otherSpecificArea = new Area { Type = AreaTypeConstants.Door };
            otherSpecificArea.Descriptions = new[] { "strong", "wood" };

            mockAreaGenerator.Setup(g => g.Generate(9266, specifications)).Returns(new[] { specificArea, otherSpecificArea });

            var areas = dungeonGenerator.GenerateFromDoor(9266, specifications);
            Assert.That(areas, Contains.Item(specificArea));
            Assert.That(areas, Contains.Item(otherSpecificArea));
            Assert.That(areas.Count(), Is.EqualTo(2));

            var last = areas.Last();
            Assert.That(last, Is.EqualTo(otherSpecificArea));
            Assert.That(otherSpecificArea.Descriptions, Contains.Item("strong"));
            Assert.That(otherSpecificArea.Descriptions, Contains.Item("wood"));
            Assert.That(otherSpecificArea.Descriptions.Count(), Is.EqualTo(2));
        }

        [Test]
        public void HallBehindDoorHasNewWidth()
        {
            var areaFromDoor = new Area();
            areaFromDoor.Type = AreaTypeConstants.Hall;
            areaFromDoor.Width = 42;
            areaFromDoor.Length = 600;
            areaFromDoor.Descriptions = new[] { "a u-turn of chaos" };

            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.DungeonAreaFromDoor)).Returns(areaFromDoor);

            var newHall = new Area();
            newHall.Length = 1337;
            newHall.Width = 1234;
            newHall.Descriptions = new[] { "some description or whatnot" };
            mockHallGenerator.Setup(g => g.Generate(9266, specifications)).Returns(new[] { newHall });

            var area = dungeonGenerator.GenerateFromDoor(9266, specifications).Single();
            Assert.That(area, Is.EqualTo(newHall));
            Assert.That(area.Descriptions, Contains.Item("some description or whatnot"));
            Assert.That(area.Descriptions, Contains.Item("a u-turn of chaos"));
            Assert.That(area.Length, Is.EqualTo(1337));
            Assert.That(area.Width, Is.EqualTo(1234));
        }
    }
}

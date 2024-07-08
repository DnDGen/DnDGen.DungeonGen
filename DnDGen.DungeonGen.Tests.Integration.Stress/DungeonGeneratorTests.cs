using DnDGen.DungeonGen.Generators;
using DnDGen.DungeonGen.Models;
using DnDGen.EncounterGen.Generators;
using DnDGen.EncounterGen.Models;
using DnDGen.Infrastructure.Selectors.Collections;
using DnDGen.RollGen;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.DungeonGen.Tests.Integration.Stress
{
    [TestFixture]
    public class DungeonGeneratorTests : StressTests
    {
        private IDungeonGenerator dungeonGenerator;
        private Dice dice;
        private ICollectionSelector collectionSelector;

        private const int FromHall = 1;
        private const int FromDoor = 0;
        private const int FromRandom = -1;

        private readonly IEnumerable<string> allTemperatures;
        private readonly IEnumerable<string> allChambers;
        private readonly IEnumerable<string> allEnvironments;
        private readonly IEnumerable<string> allTimesOfDay;

        public DungeonGeneratorTests()
        {
            allEnvironments =
               [
                EnvironmentConstants.Aquatic,
                EnvironmentConstants.Civilized,
                EnvironmentConstants.Desert,
                EnvironmentConstants.Forest,
                EnvironmentConstants.Hills,
                EnvironmentConstants.Marsh,
                EnvironmentConstants.Mountain,
                EnvironmentConstants.Plains,
                EnvironmentConstants.Underground,
            ];

            allTemperatures =
            [
                EnvironmentConstants.Temperatures.Cold,
                EnvironmentConstants.Temperatures.Temperate,
                EnvironmentConstants.Temperatures.Warm
            ];

            allChambers =
            [
                AreaTypeConstants.Chamber,
                AreaTypeConstants.Room,
            ];

            allTimesOfDay =
            [
                EnvironmentConstants.TimesOfDay.Day,
                EnvironmentConstants.TimesOfDay.Night,
            ];
        }

        [SetUp]
        public void Setup()
        {
            dungeonGenerator = GetNewInstanceOf<IDungeonGenerator>();
            dice = GetNewInstanceOf<Dice>();
            collectionSelector = GetNewInstanceOf<ICollectionSelector>();
        }

        [Test]
        public void StressDungeonGeneratorFromDoor()
        {
            stressor.Stress(() => AssertRandomArea(FromDoor));
        }

        [Test]
        public void StressDungeonGeneratorFromHall()
        {
            stressor.Stress(() => AssertRandomArea(FromHall));
        }

        protected void AssertRandomArea(int fromHall)
        {
            var areas = GenerateAreas(fromHall);
            AssertAreas(areas);
        }

        private IEnumerable<Area> GenerateAreas(int fromHall = FromRandom, int presetPartyLevel = 0)
        {
            var dungeonLevel = dice.Roll().d20().AsSum();
            var specifications = RandomizeSpecifications(presetPartyLevel);

            if (fromHall == FromRandom)
                fromHall = dice.Roll().d2().Minus(1).AsSum();

            var actuallyFromHall = Convert.ToBoolean(fromHall);

            if (actuallyFromHall)
                return dungeonGenerator.GenerateFromHall(dungeonLevel, specifications);

            return dungeonGenerator.GenerateFromDoor(dungeonLevel, specifications);
        }

        private EncounterSpecifications RandomizeSpecifications(int level = 0)
        {
            var specifications = new EncounterSpecifications();
            specifications.Environment = GetRandomFrom(allEnvironments);
            specifications.Temperature = GetRandomFrom(allTemperatures);
            specifications.TimeOfDay = GetRandomFrom(allTimesOfDay);
            specifications.Level = level > 0 ? level : dice.Roll().d20().AsSum();
            specifications.AllowAquatic = dice.Roll().d2().AsTrueOrFalse();
            specifications.AllowUnderground = dice.Roll().d2().AsTrueOrFalse();

            return specifications;
        }

        private string GetRandomFrom(IEnumerable<string> collection)
        {
            return collectionSelector.SelectRandomFrom(collection);
        }

        private void AssertAreas(IEnumerable<Area> areas)
        {
            Assert.That(areas, Is.Not.Empty);

            foreach (var area in areas)
                AssertArea(area);

            var chambers = areas.Select(a => a.Type).Intersect(allChambers);
            if (chambers.Any())
            {
                var primaryExit = chambers.First() == AreaTypeConstants.Room ? AreaTypeConstants.Door : AreaTypeConstants.Hall;
                AssertExits(areas, primaryExit);
            }
        }

        private void AssertArea(Area area)
        {
            Assert.That(area, Is.Not.Null);
            Assert.That(area.Type, Is.Not.Empty);
            Assert.That(area.Contents, Is.Not.Null);
            Assert.That(area.Descriptions, Is.Not.Null);
            Assert.That(area.Descriptions, Is.All.Not.Null);
            Assert.That(area.Descriptions, Is.All.Not.Empty);
            Assert.That(area.Contents.Encounters, Is.All.Not.Null);
            Assert.That(area.Contents.Miscellaneous, Is.All.Not.Null);
            Assert.That(area.Contents.Miscellaneous, Is.All.Not.Empty);
            Assert.That(area.Contents.Traps, Is.All.Not.Null);
            Assert.That(area.Contents.Treasures, Is.All.Not.Null);

            foreach (var encounter in area.Contents.Encounters)
            {
                Assert.That(encounter.Creatures, Is.Not.Empty);
                Assert.That(encounter.Creatures, Is.All.Not.Null);
                Assert.That(encounter.Characters, Is.Not.Null);
                Assert.That(encounter.Characters, Is.All.Not.Null);
                Assert.That(encounter.Treasures, Is.Not.Null);
                Assert.That(encounter.Treasures, Is.All.Not.Null);
                Assert.That(encounter.Treasures.Select(t => t.IsAny), Is.All.True);
            }

            foreach (var trap in area.Contents.Traps)
            {
                Assert.That(trap.ChallengeRating, Is.Positive);
                Assert.That(trap.DisableDeviceDC, Is.Positive);
                Assert.That(trap.SearchDC, Is.Positive);
                Assert.That(trap.Name, Is.Not.Empty);
                Assert.That(trap.Descriptions, Is.Not.Empty);
                Assert.That(trap.Descriptions, Contains.Item("Mechanical").Or.Contains("Magic device").Or.Contains("Spell"));
                Assert.That(trap.Descriptions.Any(t => t.Contains("trigger")), Is.True, $"{trap.Name} lacks trigger");
                Assert.That(trap.Descriptions.Any(t => t.Contains("reset")), Is.True, $"{trap.Name} lacks reset");
                Assert.That(trap.Descriptions.Count, Is.AtLeast(4));
            }

            foreach (var treasure in area.Contents.Treasures)
            {
                Assert.That(treasure.Concealment, Is.Not.Null);
                Assert.That(treasure.Container, Is.Not.Null);
                Assert.That(treasure.Treasure, Is.Not.Null);
            }

            switch (area.Type)
            {
                case AreaTypeConstants.Door: AssertDoor(area); break;
                case AreaTypeConstants.Hall: AssertHall(area); break;
                case AreaTypeConstants.Room:
                case AreaTypeConstants.Cave:
                case AreaTypeConstants.Chamber: AssertChamber(area); break;
                case AreaTypeConstants.DeadEnd: AssertDeadEnd(area); break;
                case AreaTypeConstants.Stairs: AssertStairs(area); break;
                case AreaTypeConstants.General: AssertGeneralArea(area); break;
                case AreaTypeConstants.Turn: AssertTurn(area); break;
                default: throw new ArgumentException($"Untested area type {area.Type}");
            }
        }

        private void AssertTurn(Area turn)
        {
            Assert.That(turn.Type, Is.EqualTo(AreaTypeConstants.Turn));
            Assert.That(turn.Contents.IsEmpty, Is.True);
            Assert.That(turn.Length, Is.EqualTo(30));
            Assert.That(turn.Width, Is.EqualTo(0));
            Assert.That(turn.Descriptions, Is.Not.Empty);
            Assert.That(turn.Descriptions.Count, Is.EqualTo(1));
        }

        private void AssertGeneralArea(Area area)
        {
            Assert.That(area.Type, Is.EqualTo(AreaTypeConstants.General));
            Assert.That(area.Contents.IsEmpty, Is.False);
            Assert.That(area.Length, Is.EqualTo(0));
            Assert.That(area.Width, Is.EqualTo(0));
            Assert.That(area.Descriptions, Is.Empty);
        }

        private void AssertStairs(Area stairs)
        {
            Assert.That(stairs.Type, Is.EqualTo(AreaTypeConstants.Stairs));
            Assert.That(stairs.Contents.IsEmpty, Is.True);
            Assert.That(stairs.Descriptions, Is.Not.Empty);
            Assert.That(stairs.Descriptions.Count, Is.AtLeast(1));
            Assert.That(stairs.Descriptions.Any(d => d.Contains("level")), Is.True);
            Assert.That(stairs.Length, Is.EqualTo(0));
            Assert.That(stairs.Width, Is.EqualTo(0));
        }

        private void AssertDeadEnd(Area deadEnd)
        {
            Assert.That(deadEnd.Type, Is.EqualTo(AreaTypeConstants.DeadEnd));
            Assert.That(deadEnd.Contents.IsEmpty, Is.True);
            Assert.That(deadEnd.Length, Is.EqualTo(0));
            Assert.That(deadEnd.Width, Is.EqualTo(0));
            Assert.That(deadEnd.Descriptions.FirstOrDefault(), Is.Null.Or.EqualTo("Check for secret doors along already mapped walls"));
            Assert.That(deadEnd.Descriptions.Count(), Is.AtMost(1));
        }

        private void AssertDoor(Area door)
        {
            Assert.That(door.Type, Is.EqualTo(AreaTypeConstants.Door));
            Assert.That(door.Length, Is.EqualTo(0));
            Assert.That(door.Width, Is.EqualTo(0));

            //INFO: 2 because there should be at least a location and 1 description of the door itself (usually more)
            Assert.That(door.Descriptions.Count(), Is.AtLeast(2));
        }

        private void AssertHall(Area hall)
        {
            Assert.That(hall.Type, Is.EqualTo(AreaTypeConstants.Hall));
            Assert.That(hall.Length, Is.AtLeast(30));
            Assert.That(hall.Length % 10, Is.EqualTo(0));

            //INFO: Width of 0 means it continues the same width as before
            Assert.That(hall.Width, Is.Not.Negative);
            Assert.That(hall.Width % 5, Is.EqualTo(0), $"Width: {hall.Width}");

            if (hall.Width != 5)
                Assert.That(hall.Width % 10, Is.EqualTo(0), $"Width: {hall.Width}");
        }

        private void AssertChamber(Area chamber)
        {
            Assert.That(chamber.Type, Is.EqualTo(AreaTypeConstants.Chamber)
                .Or.EqualTo(AreaTypeConstants.Cave)
                .Or.EqualTo(AreaTypeConstants.Room));

            Assert.That(chamber.Length, Is.AtLeast(10));
            Assert.That(chamber.Width, Is.Positive);
        }

        [Test]
        public void BUG_ContinuingHallHasSamePassageWidth()
        {
            //INFO: Setting the party level to 1 so that encounters, if generated, will be minimal
            var areas = stressor.GenerateOrFail(
                () => GenerateAreas(FromHall, 1),
                aa => aa.Count() == 1 && aa.Single().Type == AreaTypeConstants.Hall);

            AssertAreas(areas);

            var hall = areas.Single();
            Assert.That(hall.Type == AreaTypeConstants.Hall);
            Assert.That(hall.Width, Is.EqualTo(0));
        }

        private void AssertExits(IEnumerable<Area> areas, string primaryExitType)
        {
            var exits = areas.Where(a => a.Type == AreaTypeConstants.Door || a.Type == AreaTypeConstants.Hall);
            var halls = exits.Where(e => e.Type == AreaTypeConstants.Hall);
            var doors = exits.Where(e => e.Type == AreaTypeConstants.Door);

            foreach (var hall in halls)
                AssertHallExit(hall);

            foreach (var door in doors)
                AssertDoorExit(door);

            var areaTypes = areas.Select(a => a.Type);
            Assert.That(exits.Count(e => e.Type != primaryExitType), Is.AtMost(1), string.Join(", ", areaTypes));
        }

        private void AssertDoorExit(Area door)
        {
            AssertDoor(door);

            Assert.That(door.Descriptions, Contains.Item("Right wall")
                .Or.Contains("Left wall")
                .Or.Contains("Opposite wall")
                .Or.Contains("Same wall"));

            Assert.That(door.Descriptions, Is.All.Not.EqualTo("Straight ahead"));
            Assert.That(door.Descriptions, Is.All.Not.EqualTo("Left, 45 degrees"));
            Assert.That(door.Descriptions, Is.All.Not.EqualTo("Right, 45 degrees"));
        }

        private void AssertHallExit(Area hall)
        {
            AssertHall(hall);

            Assert.That(hall.Descriptions, Contains.Item("Right wall")
                .Or.Contains("Left wall")
                .Or.Contains("Opposite wall")
                .Or.Contains("Same wall"));

            Assert.That(hall.Descriptions, Contains.Item("Straight ahead")
                .Or.Contains("45 degrees left")
                .Or.Contains("45 degrees right"));
        }

        [Test]
        public void BUG_TrapWithRollHappens()
        {
            //INFO: Setting the party level to 1 so that encounters, if generated, will be minimal
            var areas = stressor.GenerateOrFail(
                () => GenerateAreas(presetPartyLevel: 1),
                aa => aa.Any(a => a.Contents.Traps.Any(t => t.Descriptions.Any(d => dice.ContainsRoll(d)))));
            AssertAreas(areas);

            var traps = areas.SelectMany(a => a.Contents.Traps);
            var trapsWithRolls = traps.Where(t => t.Descriptions.Any(d => dice.ContainsRoll(d)));
            Assert.That(trapsWithRolls, Is.Not.Empty);
        }
    }
}

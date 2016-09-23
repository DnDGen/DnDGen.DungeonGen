using EncounterGen.Common;
using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Tests.Integration.Stress
{
    [TestFixture]
    public class DungeonGeneratorTests : StressTests
    {
        [Inject]
        public IDungeonGenerator DungeonGenerator { get; set; }
        [Inject]
        public Random Random { get; set; }

        [TestCase(true)]
        [TestCase(false)]
        public void StressDungeonGenerator(bool fromHall)
        {
            Stress(() => AssertRandomArea(fromHall));
        }

        protected void AssertRandomArea(bool fromHall)
        {
            var areas = GenerateAreas(fromHall);
            AssertAreas(areas);
        }

        private IEnumerable<Area> GenerateAreas(bool fromHall)
        {
            var dungeonLevel = Random.Next(20) + 1;
            var partyLevel = Random.Next(20) + 1;
            var temperature = GetRandomTemperature();

            if (fromHall)
                return DungeonGenerator.GenerateFromHall(dungeonLevel, partyLevel, temperature);

            return DungeonGenerator.GenerateFromDoor(dungeonLevel, partyLevel, temperature);
        }

        private string GetRandomTemperature()
        {
            var temperatures = new[] { EnvironmentConstants.Temperatures.Cold, EnvironmentConstants.Temperatures.Temperate, EnvironmentConstants.Temperatures.Warm };
            var index = Random.Next(temperatures.Count());
            return temperatures.ElementAt(index);
        }

        private void AssertAreas(IEnumerable<Area> areas)
        {
            Assert.That(areas, Is.Not.Empty);

            foreach (var area in areas)
                AssertArea(area);
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
                Assert.That(encounter.Characters, Is.Not.Null);
                Assert.That(encounter.Characters, Is.All.Not.Null);
                Assert.That(encounter.Treasures, Is.Not.Null);
                Assert.That(encounter.Treasures, Is.All.Not.Null);
                Assert.That(encounter.Treasures.Select(t => t.IsAny), Is.All.True);
            }

            foreach (var trap in area.Contents.Traps)
            {
                Assert.That(trap.ChallengeRating, Is.Positive);
                Assert.That(trap.Descriptions, Is.Not.Empty);
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
            var areas = GenerateOrFail(() => GenerateAreas(true),
                aa => aa.Count() == 1 && aa.Single().Type == AreaTypeConstants.Hall);

            AssertAreas(areas);

            var hall = areas.Single();
            Assert.That(hall.Width, Is.EqualTo(0));
        }

        [TestCase(true, AreaTypeConstants.Chamber, AreaTypeConstants.Hall)]
        [TestCase(false, AreaTypeConstants.Chamber, AreaTypeConstants.Hall)]
        [TestCase(true, AreaTypeConstants.Room, AreaTypeConstants.Door, IgnoreReason = "Halls can't generate rooms - they generate doors which can generate rooms")]
        [TestCase(false, AreaTypeConstants.Room, AreaTypeConstants.Door)]
        public void StressExits(bool fromHall, string areaType, string primaryExitType)
        {
            Stress(() => AssertExits(fromHall, areaType, primaryExitType));
        }

        private void AssertExits(bool fromHall, string areaType, string primaryExitType)
        {
            var areas = Generate(() => GenerateAreas(fromHall),
                aa => aa.Any(a => a.Type == areaType));

            AssertAreas(areas);

            var exits = areas.Where(a => a.Type == AreaTypeConstants.Door || a.Type == AreaTypeConstants.Hall);
            var halls = exits.Where(e => e.Type == AreaTypeConstants.Hall);
            var doors = exits.Where(e => e.Type == AreaTypeConstants.Door);

            foreach (var hall in halls)
                AssertHallExit(hall);

            foreach (var door in doors)
                AssertDoorExit(door);

            Assert.That(exits.Count(e => e.Type != primaryExitType), Is.AtMost(1));
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

        [TestCase(true)]
        [TestCase(false)]
        public void ContentsHappen(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.Any(a => a.Contents.IsEmpty == false));
            AssertAreas(areas);

            Assert.That(areas.Select(a => a.Contents.IsEmpty), Is.Not.All.True);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ContentsDoNotHappen(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.All(a => a.Contents.IsEmpty));
            AssertAreas(areas);

            Assert.That(areas.Select(a => a.Contents.IsEmpty), Is.All.True);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void EncountersHappen(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.Any(a => a.Contents.Encounters.Any()));
            AssertAreas(areas);

            Assert.That(areas.Select(a => a.Contents.Encounters.Any()), Is.Not.All.False);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void EncountersDoNotHappen(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.All(a => a.Contents.Encounters.Any() == false));
            AssertAreas(areas);

            Assert.That(areas.Select(a => a.Contents.Encounters.Any()), Is.All.False);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void TrapsHappen(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.Any(a => a.Contents.Traps.Any()));
            AssertAreas(areas);

            Assert.That(areas.Select(a => a.Contents.Traps.Any()), Is.Not.All.False);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void TrapsDoNotHappen(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.All(a => a.Contents.Traps.Any() == false));
            AssertAreas(areas);

            Assert.That(areas.Select(a => a.Contents.Traps.Any()), Is.All.False);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void DungeonTreasuresHappen(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.Any(a => a.Contents.Treasures.Any()));
            AssertAreas(areas);

            Assert.That(areas.Select(a => a.Contents.Treasures.Any()), Is.Not.All.False);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void DungeonTreasuresDoNotHappen(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.All(a => a.Contents.Treasures.Any() == false));
            AssertAreas(areas);

            Assert.That(areas.Select(a => a.Contents.Treasures.Any()), Is.All.False);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ConcealedTreasuresHappen(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.Any(a => a.Contents.Treasures.Any(t => t.Concealment != string.Empty)));
            AssertAreas(areas);

            var treasures = areas.SelectMany(a => a.Contents.Treasures);
            Assert.That(treasures.Select(t => t.Concealment), Is.Not.All.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void UnconcealedTreasuresHappen(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.Any(a => a.Contents.Treasures.Any(t => t.Concealment == string.Empty)));
            AssertAreas(areas);

            var treasures = areas.SelectMany(a => a.Contents.Treasures);
            Assert.That(treasures.Select(t => t.Concealment), Is.Not.All.Not.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ContainedTreasuresHappen(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.Any(a => a.Contents.Treasures.Any(t => t.Container != string.Empty)));
            AssertAreas(areas);

            var treasures = areas.SelectMany(a => a.Contents.Treasures);
            Assert.That(treasures.Select(t => t.Container), Is.Not.All.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void UncontainedTreasuresHappen(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.Any(a => a.Contents.Treasures.Any(t => t.Container == string.Empty)));
            AssertAreas(areas);

            var treasures = areas.SelectMany(a => a.Contents.Treasures);
            Assert.That(treasures.Select(t => t.Container), Is.Not.All.Not.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void TreasureHappens(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.Any(a => a.Contents.Treasures.Any(t => t.Treasure.IsAny)));
            AssertAreas(areas);

            var treasures = areas.SelectMany(a => a.Contents.Treasures);
            Assert.That(treasures.Select(t => t.Treasure.IsAny), Is.Not.All.False);
        }

        [TestCase(true, IgnoreReason = "Whether there is any treasure is dependent on TreasureGen, not DungeonGen.  We just care that treasure does happen and that whole Dungeon Treasures might not happen")]
        [TestCase(false, IgnoreReason = "Whether there is any treasure is dependent on TreasureGen, not DungeonGen.  We just care that treasure does happen and that whole Dungeon Treasures might not happen")]
        public void TreasureDoesNotHappen(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.All(a => a.Contents.Treasures.Any(t => t.Treasure.IsAny == false)));
            AssertAreas(areas);

            var treasures = areas.SelectMany(a => a.Contents.Treasures);
            Assert.That(treasures.Select(t => t.Treasure.IsAny), Is.All.False);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void MiscellaneousContentsHappen(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.Any(a => a.Contents.Miscellaneous.Any()));
            AssertAreas(areas);

            Assert.That(areas.Select(a => a.Contents.Miscellaneous.Any()), Is.Not.All.False);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void MiscellaneousContentsDoNotHappen(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.All(a => a.Contents.Miscellaneous.Any() == false));
            AssertAreas(areas);

            Assert.That(areas.Select(a => a.Contents.Miscellaneous.Any()), Is.All.False);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void PoolHappens(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.Any(a => a.Contents.Pool != null));
            AssertAreas(areas);

            Assert.That(areas.Select(a => a.Contents.Pool), Is.Not.All.Null);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void PoolDoesNotHappen(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.All(a => a.Contents.Pool == null));
            AssertAreas(areas);

            Assert.That(areas.Select(a => a.Contents.Pool), Is.All.Null);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void MagicPoolHappens(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.Any(a => a.Contents.Pool != null && a.Contents.Pool.MagicPower != string.Empty));
            AssertAreas(areas);

            var pools = areas.Where(a => a.Contents.Pool != null).Select(a => a.Contents.Pool);
            Assert.That(pools, Is.Not.Empty);
            Assert.That(pools, Is.All.Not.Null);

            var magicPowers = pools.Select(p => p.MagicPower);
            Assert.That(magicPowers, Is.All.Not.Null);
            Assert.That(magicPowers, Is.Not.All.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void MagicPoolDoesNotHappen(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.Any(a => a.Contents.Pool != null && a.Contents.Pool.MagicPower == string.Empty));
            AssertAreas(areas);

            var pools = areas.Where(a => a.Contents.Pool != null).Select(a => a.Contents.Pool);
            Assert.That(pools, Is.Not.Empty);
            Assert.That(pools, Is.All.Not.Null);

            var magicPowers = pools.Select(p => p.MagicPower);
            Assert.That(magicPowers, Is.All.Not.Null);
            Assert.That(magicPowers, Is.All.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void PoolEncounterHappens(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.Any(a => a.Contents.Pool != null && a.Contents.Pool.Encounter != null));
            AssertAreas(areas);

            var pools = areas.Where(a => a.Contents.Pool != null).Select(a => a.Contents.Pool);
            Assert.That(pools, Is.Not.Empty);
            Assert.That(pools, Is.All.Not.Null);

            var encounters = pools.Select(p => p.Encounter);
            Assert.That(encounters, Is.All.Not.Null);
            Assert.That(encounters.Select(e => e.Creatures.Count()), Is.All.Positive);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void PoolEncounterDoesNotHappen(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.Any(a => a.Contents.Pool != null && a.Contents.Pool.Encounter == null));
            AssertAreas(areas);

            var pools = areas.Where(a => a.Contents.Pool != null).Select(a => a.Contents.Pool);
            Assert.That(pools, Is.Not.Empty);
            Assert.That(pools, Is.All.Not.Null);

            var encounters = pools.Select(p => p.Encounter);
            Assert.That(encounters, Is.All.Null);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void PoolTreasureHappens(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.Any(a => a.Contents.Pool != null && a.Contents.Pool.Treasure != null));
            AssertAreas(areas);

            var pools = areas.Where(a => a.Contents.Pool != null).Select(a => a.Contents.Pool);
            Assert.That(pools, Is.Not.Empty);
            Assert.That(pools, Is.All.Not.Null);

            var treasures = pools.Select(p => p.Treasure);
            Assert.That(treasures, Is.All.Not.Null);
            Assert.That(treasures.Select(t => t.Treasure.IsAny), Is.All.True);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void PoolTreasureDoesNotHappen(bool fromHall)
        {
            var areas = GenerateOrFail(() => GenerateAreas(fromHall), aa => aa.Any(a => a.Contents.Pool != null && a.Contents.Pool.Treasure == null));
            AssertAreas(areas);

            var pools = areas.Where(a => a.Contents.Pool != null).Select(a => a.Contents.Pool);
            Assert.That(pools, Is.Not.Empty);
            Assert.That(pools, Is.All.Not.Null);

            var treasures = pools.Select(p => p.Treasure);
            Assert.That(treasures, Is.All.Null);
        }
    }
}

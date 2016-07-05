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

        [TestCase("Dungeon Generator - From Hall")]
        public override void Stress(string stressSubject)
        {
            Stress();
        }

        protected override void MakeAssertions()
        {
            var areas = GenerateAreas();
            Assert.That(areas, Is.Not.Empty);

            foreach (var area in areas)
            {
                AssertArea(area);

                switch (area.Type)
                {
                    case AreaTypeConstants.Door: AssertDoor(area); break;
                    case AreaTypeConstants.Hall: AssertHall(area); break;
                    case AreaTypeConstants.Room:
                    case AreaTypeConstants.Cave:
                    case AreaTypeConstants.Chamber: AssertChamber(area); break;
                    default: break;
                }
            }
        }

        private IEnumerable<Area> GenerateAreas(bool fromHall = true)
        {
            var dungeonLevel = Random.Next(20) + 1;
            var partyLevel = Random.Next(20) + 1;

            if (fromHall)
                return DungeonGenerator.GenerateFromHall(dungeonLevel, partyLevel);

            return DungeonGenerator.GenerateFromDoor(dungeonLevel, partyLevel);
        }

        private void AssertArea(Area area)
        {
            Assert.That(area, Is.Not.Null);
            Assert.That(area.Type, Is.Not.Empty);
            Assert.That(area.Contents, Is.Not.Null);
            Assert.That(area.Descriptions, Is.Not.Null);

            if (area.Type == AreaTypeConstants.General)
            {
                Assert.That(area.Contents.IsEmpty, Is.False);
            }
            else if (area.Type != AreaTypeConstants.DeadEnd && area.Type != AreaTypeConstants.Door && area.Type != AreaTypeConstants.Stairs)
            {
                Assert.That(area.Length, Is.Positive, area.Type);
                Assert.That(area.Width, Is.Not.Negative, area.Type);
            }
        }

        private void AssertDoor(Area door)
        {
            Assert.That(door.Length, Is.EqualTo(0));
            Assert.That(door.Width, Is.EqualTo(0));

            //INFO: 2 because there should be at least a location and 1 description of the door itself (usually more)
            Assert.That(door.Descriptions.Count(), Is.AtLeast(2));
        }

        private void AssertHall(Area hall)
        {
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
            Assert.That(chamber.Length, Is.AtLeast(10));
            Assert.That(chamber.Width, Is.Positive);
        }

        [Test]
        public void StressFromDoor()
        {
            Stress(AssertFromDoor);
        }

        private void AssertFromDoor()
        {
            var areas = GenerateAreas(false);
            Assert.That(areas, Is.Not.Empty);

            foreach (var area in areas)
                AssertArea(area);
        }

        [Test]
        public void ContinuingHallHasSamePassageWidth()
        {
            Stress(AssertHallContinuesAtSameWidth);
        }

        private void AssertHallContinuesAtSameWidth()
        {
            var areas = Generate(() => GenerateAreas(),
                aa => aa.Count() == 1 && aa.Single().Type == AreaTypeConstants.Hall);

            Assert.That(areas, Is.Not.Empty);

            var hall = areas.Single();
            Assert.That(hall.Width, Is.EqualTo(0));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void NewHallHasNewPassageWidth(bool fromHall)
        {
            Stress(() => AssertNewHallHasNewWidth(fromHall));
        }

        private void AssertNewHallHasNewWidth(bool fromHall)
        {
            var areas = Generate(() => GenerateAreas(fromHall),
                aa => aa.Count(a => a.Type != AreaTypeConstants.General) > 1 && aa.Any(a => a.Type == AreaTypeConstants.Hall));

            Assert.That(areas, Is.Not.Empty);

            var halls = areas.Where(a => a.Type == AreaTypeConstants.Hall);
            var hallsWithZeroWidth = halls.Where(h => h.Width == 0);

            Assert.That(hallsWithZeroWidth.Count, Is.AtMost(1));

            foreach (var hall in halls.Except(hallsWithZeroWidth))
            {
                Assert.That(hall.Width, Is.Positive);
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void StressChambers(bool fromHall)
        {
            Stress(() => AssertChambers(fromHall));
        }

        private void AssertChambers(bool fromHall)
        {
            var areas = Generate(() => GenerateAreas(fromHall),
                aa => aa.Any(a => a.Type == AreaTypeConstants.Chamber));

            foreach (var area in areas)
                AssertArea(area);

            var chambers = areas.Where(a => a.Type == AreaTypeConstants.Chamber);

            foreach (var chamber in chambers)
            {
                Assert.That(chamber.Length, Is.AtLeast(10));
                Assert.That(chamber.Width, Is.Positive);
            }

            var exits = areas.Where(a => a.Type == AreaTypeConstants.Door || a.Type == AreaTypeConstants.Hall);

            var doors = exits.Where(e => e.Type == AreaTypeConstants.Door);
            Assert.That(doors.Count(), Is.AtMost(1));

            if (doors.Any())
            {
                var door = doors.Single();
                AssertDoorExit(door);
            }

            var halls = exits.Where(e => e.Type == AreaTypeConstants.Hall);

            foreach (var hall in halls)
                AssertHallExit(hall);
        }

        private void AssertDoorExit(Area door)
        {
            Assert.That(door.Length, Is.EqualTo(0));
            Assert.That(door.Width, Is.EqualTo(0));

            //INFO: 2 because there should be at least a location and 1 description of the door itself (usually more)
            Assert.That(door.Descriptions.Count(), Is.AtLeast(2));
            Assert.That(door.Descriptions, Contains.Item("Right wall")
                .Or.Contains("Left wall")
                .Or.Contains("Opposite wall")
                .Or.Contains("Same wall"));

            Assert.That(door.Descriptions, Is.Not.Contains("Straight ahead"));
            Assert.That(door.Descriptions, Is.Not.Contains("Left, 45 degrees"));
            Assert.That(door.Descriptions, Is.Not.Contains("Right, 45 degrees"));
        }

        private void AssertHallExit(Area hall)
        {
            Assert.That(hall.Length, Is.AtLeast(30));
            Assert.That(hall.Width, Is.AtLeast(5));

            Assert.That(hall.Descriptions, Contains.Item("Right wall")
                .Or.Contains("Left wall")
                .Or.Contains("Opposite wall")
                .Or.Contains("Same wall"));

            Assert.That(hall.Descriptions, Contains.Item("Straight ahead")
                .Or.Contains("45 degrees left")
                .Or.Contains("45 degrees right"));
        }

        [TestCase(true, IgnoreReason = "Halls can't generate rooms - they generate doors which can generate rooms")]
        [TestCase(false)]
        public void StressRooms(bool fromHall)
        {
            Stress(() => AssertRooms(fromHall));
        }

        private void AssertRooms(bool fromHall)
        {
            var areas = Generate(() => GenerateAreas(fromHall),
                aa => aa.Any(a => a.Type == AreaTypeConstants.Room));

            foreach (var area in areas)
                AssertArea(area);

            var rooms = areas.Where(a => a.Type == AreaTypeConstants.Room);

            foreach (var room in rooms)
            {
                Assert.That(room.Length, Is.AtLeast(10));
                Assert.That(room.Width, Is.Positive);
            }

            var exits = areas.Where(a => a.Type == AreaTypeConstants.Door || a.Type == AreaTypeConstants.Hall);

            var halls = exits.Where(e => e.Type == AreaTypeConstants.Hall);
            Assert.That(halls.Count(), Is.AtMost(1));

            if (halls.Any())
            {
                var hall = halls.Single();
                AssertHallExit(hall);
            }

            var doors = exits.Where(e => e.Type == AreaTypeConstants.Door);

            foreach (var door in doors)
                AssertDoorExit(door);
        }
    }
}

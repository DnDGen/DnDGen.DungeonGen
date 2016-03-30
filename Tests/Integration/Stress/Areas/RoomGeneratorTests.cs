using DungeonGen.Common;
using DungeonGen.Generators;
using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Tests.Integration.Stress.Areas
{
    [TestFixture]
    public class RoomGeneratorTests : StressTests
    {
        [Inject, Named(AreaTypeConstants.Room)]
        public AreaGenerator RoomGenerator { get; set; }
        [Inject]
        public Random Random { get; set; }

        [TestCase("Room Generator")]
        public override void Stress(string stressSubject)
        {
            Stress();
        }

        protected override void MakeAssertions()
        {
            var areas = GenerateRoom();
            Assert.That(areas, Is.Not.Empty);

            foreach (var area in areas)
                AssertArea(area);

            var rooms = areas.Where(a => a.Type == AreaTypeConstants.Room || a.Type == AreaTypeConstants.Cave);

            foreach (var room in rooms)
            {
                Assert.That(room.Length, Is.AtLeast(10));
                Assert.That(room.Width, Is.Positive);
            }

            var exits = areas.Except(rooms);

            foreach (var exit in exits)
            {
                Assert.That(exit.Type, Is.EqualTo(AreaTypeConstants.Door).Or.EqualTo(AreaTypeConstants.Hall));
            }

            var doors = exits.Where(e => e.Type == AreaTypeConstants.Door);

            foreach (var door in doors)
            {
                Assert.That(door.Length, Is.EqualTo(0));
                Assert.That(door.Width, Is.EqualTo(0));

                //INFO: 2 because there should be at least a location and 1 description of the door itself (usually more)
                Assert.That(door.Descriptions.Count(), Is.AtLeast(2));
            }

            var halls = exits.Where(e => e.Type == AreaTypeConstants.Hall);
            Assert.That(halls.Count(), Is.AtMost(rooms.Count()));

            foreach (var hall in halls)
            {
                Assert.That(hall.Length, Is.AtLeast(30));
                Assert.That(hall.Width, Is.AtLeast(5));
            }
        }

        private IEnumerable<Area> GenerateRoom()
        {
            var dungeonLevel = Random.Next(20) + 1;
            var partyLevel = Random.Next(20) + 1;
            return RoomGenerator.Generate(dungeonLevel, partyLevel);
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
    }
}

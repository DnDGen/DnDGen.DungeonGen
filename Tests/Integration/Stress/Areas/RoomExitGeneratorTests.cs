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
    public class RoomExitGeneratorTests : StressTests
    {
        [Inject, Named(AreaTypeConstants.Room)]
        public ExitGenerator RoomExitGenerator { get; set; }
        [Inject]
        public Random Random { get; set; }

        [TestCase("Room Exit Generator")]
        public override void Stress(string stressSubject)
        {
            Stress();
        }

        protected override void MakeAssertions()
        {
            var exits = GenerateRoomExits();

            foreach (var area in exits)
                AssertArea(area);

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
            Assert.That(halls.Count(), Is.AtMost(1));

            if (halls.Any())
            {
                var hall = halls.Single();
                Assert.That(hall.Length, Is.AtLeast(30));
                Assert.That(hall.Width, Is.AtLeast(5));
            }
        }

        private IEnumerable<Area> GenerateRoomExits()
        {
            var dungeonLevel = Random.Next(20) + 1;
            var partyLevel = Random.Next(20) + 1;
            var length = Random.Next(10, 51);
            var width = Random.Next(10, 51);

            return RoomExitGenerator.Generate(dungeonLevel, partyLevel, length, width);
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

        [Test]
        public void RoomDoorsHaveLocationAndNoDirection()
        {
            Stress(AssertRoomDoorsHaveLocationAndNoDirection);
        }

        private void AssertRoomDoorsHaveLocationAndNoDirection()
        {
            var exits = GenerateRoomExits();
            var doors = exits.Where(a => a.Type == AreaTypeConstants.Door);

            foreach (var door in doors)
            {
                Assert.That(door.Descriptions, Contains.Item("Right wall")
                    .Or.Contains("Left wall")
                    .Or.Contains("Opposite wall")
                    .Or.Contains("Same wall"));

                Assert.That(door.Descriptions, Is.Not.Contains("Straight ahead"));
                Assert.That(door.Descriptions, Is.Not.Contains("Left, 45 degrees"));
                Assert.That(door.Descriptions, Is.Not.Contains("Right, 45 degrees"));
            }
        }

        [Test]
        public void RoomHallsHaveLocationAndDirection()
        {
            Stress(AssertRoomHallsHaveLocationAndDirection);
        }

        private void AssertRoomHallsHaveLocationAndDirection()
        {
            var exits = GenerateRoomExits();
            var halls = exits.Where(a => a.Type == AreaTypeConstants.Hall);

            foreach (var hall in halls)
            {
                Assert.That(hall.Descriptions, Contains.Item("Right wall")
                    .Or.Contains("Left wall")
                    .Or.Contains("Opposite wall")
                    .Or.Contains("Same wall"));

                Assert.That(hall.Descriptions, Contains.Item("Straight ahead")
                    .Or.Contains("45 degrees left")
                    .Or.Contains("45 degrees right"));
            }
        }
    }
}

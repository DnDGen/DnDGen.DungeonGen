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
    public class ChamberExitGeneratorTests : StressTests
    {
        [Inject, Named(AreaTypeConstants.Chamber)]
        public ExitGenerator ChamberExitGenerator { get; set; }
        [Inject]
        public Random Random { get; set; }

        [TestCase("Chamber Exit Generator")]
        public override void Stress(string stressSubject)
        {
            Stress();
        }

        protected override void MakeAssertions()
        {
            var exits = GenerateChamberExits();

            foreach (var area in exits)
                AssertArea(area);

            foreach (var exit in exits)
            {
                Assert.That(exit.Type, Is.EqualTo(AreaTypeConstants.Door).Or.EqualTo(AreaTypeConstants.Hall));
            }

            var doors = exits.Where(e => e.Type == AreaTypeConstants.Door);
            Assert.That(doors.Count(), Is.AtMost(1));

            if (doors.Any())
            {
                var door = doors.Single();
                Assert.That(door.Length, Is.EqualTo(0));
                Assert.That(door.Width, Is.EqualTo(0));

                //INFO: 2 because there should be at least a location and 1 description of the door itself (usually more)
                Assert.That(door.Descriptions.Count(), Is.AtLeast(2));
            }

            var halls = exits.Where(e => e.Type == AreaTypeConstants.Hall);

            foreach (var hall in halls)
            {
                Assert.That(hall.Length, Is.AtLeast(30));
                Assert.That(hall.Width, Is.AtLeast(5));
            }
        }

        private IEnumerable<Area> GenerateChamberExits()
        {
            var dungeonLevel = Random.Next(20) + 1;
            var partyLevel = Random.Next(20) + 1;
            var length = Random.Next(10, 51);
            var width = Random.Next(10, 51);

            return ChamberExitGenerator.Generate(dungeonLevel, partyLevel, length, width);
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
        public void ChamberDoorsHaveLocationAndNoDirection()
        {
            Stress(AssertChamberDoorsHaveLocationAndNoDirection);
        }

        private void AssertChamberDoorsHaveLocationAndNoDirection()
        {
            var exits = GenerateChamberExits();
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
        public void ChamberHallsHaveLocationAndDirection()
        {
            Stress(AssertChamberHallsHaveLocationAndDirection);
        }

        private void AssertChamberHallsHaveLocationAndDirection()
        {
            var exits = GenerateChamberExits();
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

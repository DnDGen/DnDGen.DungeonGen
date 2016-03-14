using DungeonGen.Common;
using DungeonGen.Generators;
using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DungeonGen.Tests.Integration.Stress
{
    [TestFixture]
    public class DungeonGeneratorTests : StressTests
    {
        [Inject]
        public IDungeonGenerator DungeonGenerator { get; set; }
        [Inject]
        public Random Random { get; set; }

        [TestCase("Dungeon generator - From Hall")]
        public override void Stress(string stressSubject)
        {
            Stress();
        }

        protected override void MakeAssertions()
        {
            var areas = GenerateFromHall();
            Assert.That(areas, Is.Not.Empty);

            foreach (var area in areas)
                AssertArea(area);
        }

        private IEnumerable<Area> GenerateFromHall()
        {
            var level = Random.Next(20) + 1;
            return DungeonGenerator.GenerateFromHall(level);
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
            else if (area.Type != AreaTypeConstants.DeadEnd)
            {
                Assert.That(area.Length, Is.Positive, area.Type);
                Assert.That(area.Width, Is.Not.Negative, area.Type);
            }
        }

        [Test]
        public void StressFromDoor()
        {
            Stress(AssertFromDoor);
        }

        private void AssertFromDoor()
        {
            var level = Random.Next(20) + 1;
            var areas = DungeonGenerator.GenerateFromDoor(level);
            Assert.That(areas, Is.Not.Empty);

            foreach (var area in areas)
                AssertArea(area);
        }
    }
}

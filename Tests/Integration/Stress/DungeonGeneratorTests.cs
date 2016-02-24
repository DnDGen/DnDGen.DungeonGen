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
            AssertAreas(areas);
        }

        private IEnumerable<Area> GenerateFromHall()
        {
            var level = Random.Next(20) + 1;
            return DungeonGenerator.GenerateFromHall(level);
        }

        private void AssertAreas(IEnumerable<Area> areas)
        {
            foreach (var area in areas)
            {
                Assert.That(area, Is.Not.Null);
                Assert.That(area.Type, Is.Not.Empty);
                Assert.That(area.Length, Is.Positive);
                Assert.That(area.Width, Is.Positive);
                Assert.That(area.Contents, Is.Not.Null);
                Assert.That(area.Description, Is.Not.Null);

                if (area.Type == AreaTypeConstants.General)
                {
                    Assert.That(area.Contents.IsEmpty, Is.False);
                }
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
            AssertAreas(areas);
        }
    }
}

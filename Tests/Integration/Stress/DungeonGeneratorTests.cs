using DungeonGen.Common;
using DungeonGen.Generators;
using Ninject;
using NUnit.Framework;
using System;

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
            var area = GenerateFromHall();
            AssertArea(area);
        }

        private Area GenerateFromHall()
        {
            var level = Random.Next(20) + 1;
            return DungeonGenerator.GenerateFromHall(level);
        }

        private void AssertArea(Area area)
        {
            Assert.That(area, Is.Not.Null);
            Assert.That(area.Type, Is.Not.Empty);
            Assert.That(area.Length, Is.Positive);
            Assert.That(area.Width, Is.Positive);
            Assert.That(area.Contents, Is.Not.Null);
            Assert.That(area.Description, Is.Not.Null);
        }

        [Test]
        public void StressFromDoor()
        {
            Stress(AssertFromDoor);
        }

        private void AssertFromDoor()
        {
            var level = Random.Next(20) + 1;
            var area = DungeonGenerator.GenerateFromDoor(level);
            AssertArea(area);
        }
    }
}

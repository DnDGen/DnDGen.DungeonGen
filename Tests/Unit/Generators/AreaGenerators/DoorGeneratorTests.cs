using DungeonGen.Common;
using DungeonGen.Generators;
using DungeonGen.Generators.Domain.AreaGenerators;
using DungeonGen.Selectors;
using DungeonGen.Tables;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace DungeonGen.Tests.Unit.Generators.AreaGenerators
{
    [TestFixture]
    public class DoorGeneratorTests
    {
        private AreaGenerator doorGenerator;
        private Mock<IPercentileSelector> mockPercentileSelector;

        [SetUp]
        public void Setup()
        {
            mockPercentileSelector = new Mock<IPercentileSelector>();
            doorGenerator = new DoorGenerator(mockPercentileSelector.Object);
        }

        [Test]
        public void GenerateDoor()
        {
            var doors = doorGenerator.Generate(9266);
            Assert.That(doors, Is.Not.Null);
            Assert.That(doors, Is.Not.Empty);
            Assert.That(doors.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GetDoorType()
        {
            mockPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.DoorType)).Returns("door-like");

            var door = doorGenerator.Generate(9266).Single();
            Assert.That(door.Contents.IsEmpty, Is.True);
            Assert.That(door.Descriptions, Contains.Item("door-like"));
            Assert.That(door.Descriptions.Count(), Is.EqualTo(1));
            Assert.That(door.Length, Is.EqualTo(0));
            Assert.That(door.Type, Is.EqualTo(AreaTypeConstants.Door));
            Assert.That(door.Width, Is.EqualTo(0));
        }
    }
}

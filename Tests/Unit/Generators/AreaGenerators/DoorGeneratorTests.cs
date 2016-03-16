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
        private Mock<IAreaPercentileSelector> mockAreaPercentileSelector;

        [SetUp]
        public void Setup()
        {
            mockAreaPercentileSelector = new Mock<IAreaPercentileSelector>();
            doorGenerator = new DoorGenerator(mockAreaPercentileSelector.Object);
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
            var selectedDoor = new Area();
            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.DoorTypes)).Returns(selectedDoor);

            var door = doorGenerator.Generate(9266).Single();
            Assert.That(door, Is.EqualTo(selectedDoor));
        }
    }
}

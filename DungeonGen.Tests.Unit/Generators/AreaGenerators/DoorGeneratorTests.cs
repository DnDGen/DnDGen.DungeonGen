using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.AreaGenerators;
using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
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
        private Area selectedDoor;

        [SetUp]
        public void Setup()
        {
            mockAreaPercentileSelector = new Mock<IAreaPercentileSelector>();
            doorGenerator = new DoorGenerator(mockAreaPercentileSelector.Object);

            selectedDoor = new Area();
            selectedDoor.Type = "door type";
            selectedDoor.Descriptions = new[] { "complicated", DescriptionConstants.Wooden };

            mockAreaPercentileSelector.Setup(s => s.SelectFrom(TableNameConstants.DoorTypes)).Returns(selectedDoor);
        }

        [Test]
        public void GenerateDoor()
        {
            var doors = doorGenerator.Generate(9266, 90210);
            Assert.That(doors, Is.Not.Null);
            Assert.That(doors, Is.Not.Empty);
            Assert.That(doors.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GetDoor()
        {
            var door = doorGenerator.Generate(9266, 90210).Single();
            Assert.That(door, Is.EqualTo(selectedDoor));
        }

        [Test]
        public void GetStuckDoor()
        {
            selectedDoor.Length = 600;

            var door = doorGenerator.Generate(9266, 90210).Single();
            Assert.That(door, Is.EqualTo(selectedDoor));
            Assert.That(door.Descriptions, Contains.Item("Stuck (Break DC 600)"));
            Assert.That(door.Descriptions, Contains.Item("complicated"));
            Assert.That(door.Descriptions, Contains.Item(DescriptionConstants.Wooden));
            Assert.That(door.Descriptions.Count(), Is.EqualTo(3));
            Assert.That(door.Length, Is.EqualTo(0));
            Assert.That(door.Width, Is.EqualTo(0));
        }

        [Test]
        public void GetLockedDoor()
        {
            selectedDoor.Width = 600;

            var door = doorGenerator.Generate(9266, 90210).Single();
            Assert.That(door, Is.EqualTo(selectedDoor));
            Assert.That(door.Descriptions, Contains.Item("Locked (Break DC 600)"));
            Assert.That(door.Descriptions, Contains.Item("complicated"));
            Assert.That(door.Descriptions, Contains.Item(DescriptionConstants.Wooden));
            Assert.That(door.Descriptions.Count(), Is.EqualTo(3));
            Assert.That(door.Length, Is.EqualTo(0));
            Assert.That(door.Width, Is.EqualTo(0));
        }

        [Test]
        public void GetSpecialDoor()
        {
            var specialDoor = new Area();
            specialDoor.Type = AreaTypeConstants.Special;
            specialDoor.Descriptions = new[] { "a very special door" };
            specialDoor.Length = 600;

            mockAreaPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DoorTypes)).Returns(specialDoor).Returns(selectedDoor);

            var door = doorGenerator.Generate(9266, 90210).Single();
            Assert.That(door, Is.EqualTo(selectedDoor));
            Assert.That(door.Descriptions, Contains.Item("a very special door"));
            Assert.That(door.Descriptions, Contains.Item("complicated"));
            Assert.That(door.Descriptions, Contains.Item(DescriptionConstants.Wooden));
            Assert.That(door.Descriptions.Count(), Is.EqualTo(3));
            Assert.That(door.Length, Is.EqualTo(0));
            Assert.That(door.Width, Is.EqualTo(0));
        }

        [Test]
        public void GetSpecialStuckDoor()
        {
            selectedDoor.Length = 90210;

            var specialDoor = new Area();
            specialDoor.Type = AreaTypeConstants.Special;
            specialDoor.Descriptions = new[] { "a very special door" };
            specialDoor.Length = 42;

            mockAreaPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DoorTypes)).Returns(specialDoor).Returns(selectedDoor);

            var door = doorGenerator.Generate(9266, 90210).Single();
            Assert.That(door, Is.EqualTo(selectedDoor));
            Assert.That(door.Descriptions, Contains.Item("Stuck (Break DC 90252)"));
            Assert.That(door.Descriptions, Contains.Item("a very special door"));
            Assert.That(door.Descriptions, Contains.Item("complicated"));
            Assert.That(door.Descriptions, Contains.Item(DescriptionConstants.Wooden));
            Assert.That(door.Descriptions.Count(), Is.EqualTo(4));
            Assert.That(door.Length, Is.EqualTo(0));
            Assert.That(door.Width, Is.EqualTo(0));
        }

        [Test]
        public void GetSpecialLockedDoor()
        {
            selectedDoor.Width = 600;

            var specialDoor = new Area();
            specialDoor.Type = AreaTypeConstants.Special;
            specialDoor.Descriptions = new[] { "a very special door" };
            specialDoor.Length = 42;

            mockAreaPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DoorTypes)).Returns(specialDoor).Returns(selectedDoor);

            var door = doorGenerator.Generate(9266, 90210).Single();
            Assert.That(door, Is.EqualTo(selectedDoor));
            Assert.That(door.Descriptions, Contains.Item("Locked (Break DC 642)"));
            Assert.That(door.Descriptions, Contains.Item("a very special door"));
            Assert.That(door.Descriptions, Contains.Item("complicated"));
            Assert.That(door.Descriptions, Contains.Item(DescriptionConstants.Wooden));
            Assert.That(door.Descriptions.Count(), Is.EqualTo(4));
            Assert.That(door.Length, Is.EqualTo(0));
            Assert.That(door.Width, Is.EqualTo(0));
        }

        [Test]
        public void GetFirstSpecialDoorAndIgnoreOtherSpecialDoors()
        {
            selectedDoor.Length = 600;

            var specialDoor = new Area();
            specialDoor.Type = AreaTypeConstants.Special;
            specialDoor.Descriptions = new[] { "a very special door" };
            specialDoor.Length = 42;

            var otherSpecialDoor = new Area();
            otherSpecialDoor.Type = AreaTypeConstants.Special;
            otherSpecialDoor.Descriptions = new[] { "a not-so-special door" };
            otherSpecialDoor.Length = 1337;

            mockAreaPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DoorTypes)).Returns(specialDoor).Returns(otherSpecialDoor).Returns(selectedDoor);

            var door = doorGenerator.Generate(9266, 90210).Single();
            Assert.That(door, Is.EqualTo(selectedDoor));
            Assert.That(door.Descriptions, Contains.Item("Stuck (Break DC 642)"));
            Assert.That(door.Descriptions, Contains.Item("a very special door"));
            Assert.That(door.Descriptions, Contains.Item("complicated"));
            Assert.That(door.Descriptions, Contains.Item(DescriptionConstants.Wooden));
            Assert.That(door.Descriptions.Count(), Is.EqualTo(4));
            Assert.That(door.Length, Is.EqualTo(0));
            Assert.That(door.Width, Is.EqualTo(0));
        }

        public void GetMagicallyReinforcedSpecialWoodenDoor()
        {
            var specialDoor = new Area();
            specialDoor.Type = AreaTypeConstants.Special;
            specialDoor.Descriptions = new[] { DescriptionConstants.MagicallyReinforced };
            specialDoor.Length = 600;
            specialDoor.Width = 42;

            mockAreaPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DoorTypes)).Returns(specialDoor).Returns(selectedDoor);

            var door = doorGenerator.Generate(9266, 90210).Single();
            Assert.That(door, Is.EqualTo(selectedDoor));
            Assert.That(door.Descriptions, Contains.Item(DescriptionConstants.MagicallyReinforced));
            Assert.That(door.Descriptions, Contains.Item("complicated"));
            Assert.That(door.Descriptions, Contains.Item(DescriptionConstants.Wooden));
            Assert.That(door.Descriptions.Count(), Is.EqualTo(3));
            Assert.That(door.Length, Is.EqualTo(0));
            Assert.That(door.Width, Is.EqualTo(0));
        }

        public void GetMagicallyReinforcedSpecialNonWoodenDoor()
        {
            selectedDoor.Descriptions = selectedDoor.Descriptions.Except(new[] { DescriptionConstants.Wooden });

            var specialDoor = new Area();
            specialDoor.Type = AreaTypeConstants.Special;
            specialDoor.Descriptions = new[] { DescriptionConstants.MagicallyReinforced };
            specialDoor.Length = 600;
            specialDoor.Width = 42;

            mockAreaPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DoorTypes)).Returns(specialDoor).Returns(selectedDoor);

            var door = doorGenerator.Generate(9266, 90210).Single();
            Assert.That(door, Is.EqualTo(selectedDoor));
            Assert.That(door.Descriptions, Contains.Item(DescriptionConstants.MagicallyReinforced));
            Assert.That(door.Descriptions, Contains.Item("complicated"));
            Assert.That(door.Descriptions.Count(), Is.EqualTo(2));
            Assert.That(door.Length, Is.EqualTo(0));
            Assert.That(door.Width, Is.EqualTo(0));
        }

        [Test]
        public void GetMagicallyReinforcedSpecialStuckWoodenDoor()
        {
            selectedDoor.Length = 600;

            var specialDoor = new Area();
            specialDoor.Type = AreaTypeConstants.Special;
            specialDoor.Descriptions = new[] { DescriptionConstants.MagicallyReinforced };
            specialDoor.Length = 1337;
            specialDoor.Width = 42;

            mockAreaPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DoorTypes)).Returns(specialDoor).Returns(selectedDoor);

            var door = doorGenerator.Generate(9266, 90210).Single();
            Assert.That(door, Is.EqualTo(selectedDoor));
            Assert.That(door.Descriptions, Contains.Item("Stuck (Break DC 1337)"));
            Assert.That(door.Descriptions, Contains.Item(DescriptionConstants.MagicallyReinforced));
            Assert.That(door.Descriptions, Contains.Item("complicated"));
            Assert.That(door.Descriptions, Contains.Item(DescriptionConstants.Wooden));
            Assert.That(door.Descriptions.Count(), Is.EqualTo(4));
            Assert.That(door.Length, Is.EqualTo(0));
            Assert.That(door.Width, Is.EqualTo(0));
        }

        [Test]
        public void GetMagicallyReinforcedSpecialStuckNonWoodenDoor()
        {
            selectedDoor.Descriptions = selectedDoor.Descriptions.Except(new[] { DescriptionConstants.Wooden });
            selectedDoor.Length = 600;

            var specialDoor = new Area();
            specialDoor.Type = AreaTypeConstants.Special;
            specialDoor.Descriptions = new[] { DescriptionConstants.MagicallyReinforced };
            specialDoor.Length = 1337;
            specialDoor.Width = 42;

            mockAreaPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DoorTypes)).Returns(specialDoor).Returns(selectedDoor);

            var door = doorGenerator.Generate(9266, 90210).Single();
            Assert.That(door, Is.EqualTo(selectedDoor));
            Assert.That(door.Descriptions, Contains.Item("Stuck (Break DC 42)"));
            Assert.That(door.Descriptions, Contains.Item(DescriptionConstants.MagicallyReinforced));
            Assert.That(door.Descriptions, Contains.Item("complicated"));
            Assert.That(door.Descriptions.Count(), Is.EqualTo(3));
            Assert.That(door.Length, Is.EqualTo(0));
            Assert.That(door.Width, Is.EqualTo(0));
        }

        [Test]
        public void GetMagicallyReinforcedSpecialLockedWoodenDoor()
        {
            selectedDoor.Width = 600;

            var specialDoor = new Area();
            specialDoor.Type = AreaTypeConstants.Special;
            specialDoor.Descriptions = new[] { DescriptionConstants.MagicallyReinforced };
            specialDoor.Length = 1337;
            specialDoor.Width = 42;

            mockAreaPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DoorTypes)).Returns(specialDoor).Returns(selectedDoor);

            var door = doorGenerator.Generate(9266, 90210).Single();
            Assert.That(door, Is.EqualTo(selectedDoor));
            Assert.That(door.Descriptions, Contains.Item("Locked (Break DC 1337)"));
            Assert.That(door.Descriptions, Contains.Item(DescriptionConstants.MagicallyReinforced));
            Assert.That(door.Descriptions, Contains.Item("complicated"));
            Assert.That(door.Descriptions, Contains.Item(DescriptionConstants.Wooden));
            Assert.That(door.Descriptions.Count(), Is.EqualTo(4));
            Assert.That(door.Length, Is.EqualTo(0));
            Assert.That(door.Width, Is.EqualTo(0));
        }

        [Test]
        public void GetMagicallyReinforcedSpecialLockedNonWoodenDoor()
        {
            selectedDoor.Descriptions = selectedDoor.Descriptions.Except(new[] { DescriptionConstants.Wooden });
            selectedDoor.Width = 600;

            var specialDoor = new Area();
            specialDoor.Type = AreaTypeConstants.Special;
            specialDoor.Descriptions = new[] { DescriptionConstants.MagicallyReinforced };
            specialDoor.Length = 1337;
            specialDoor.Width = 42;

            mockAreaPercentileSelector.SetupSequence(s => s.SelectFrom(TableNameConstants.DoorTypes)).Returns(specialDoor).Returns(selectedDoor);

            var door = doorGenerator.Generate(9266, 90210).Single();
            Assert.That(door, Is.EqualTo(selectedDoor));
            Assert.That(door.Descriptions, Contains.Item("Locked (Break DC 42)"));
            Assert.That(door.Descriptions, Contains.Item(DescriptionConstants.MagicallyReinforced));
            Assert.That(door.Descriptions, Contains.Item("complicated"));
            Assert.That(door.Descriptions.Count(), Is.EqualTo(3));
            Assert.That(door.Length, Is.EqualTo(0));
            Assert.That(door.Width, Is.EqualTo(0));
        }
    }
}

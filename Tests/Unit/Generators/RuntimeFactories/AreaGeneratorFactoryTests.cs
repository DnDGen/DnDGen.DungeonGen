using DungeonGen.Common;
using DungeonGen.Generators.Domain.AreaGenerators;
using DungeonGen.Generators.Domain.RuntimeFactories;
using DungeonGen.Generators.Domain.RuntimeFactories.Domain;
using NUnit.Framework;

namespace DungeonGen.Tests.Unit.Generators.RuntimeFactories
{
    [TestFixture]
    public class AreaGeneratorFactoryTests
    {
        private IAreaGeneratorFactory areaGeneratorFactory;

        [SetUp]
        public void Setup()
        {
            areaGeneratorFactory = new AreaGeneratorFactory();
        }

        [Test]
        public void BuildDoorGenerator()
        {
            var generator = areaGeneratorFactory.Build(AreaTypeConstants.Door);
            Assert.That(generator, Is.Not.Null);
            Assert.That(generator, Is.InstanceOf<DoorGenerator>());
        }

        [Test]
        public void BuildChamberGenerator()
        {
            var generator = areaGeneratorFactory.Build(AreaTypeConstants.Chamber);
            Assert.That(generator, Is.Not.Null);
            Assert.That(generator, Is.InstanceOf<ChamberGenerator>());
        }

        [Test]
        public void BuildSidePassageGenerator()
        {
            var generator = areaGeneratorFactory.Build(AreaTypeConstants.SidePassage);
            Assert.That(generator, Is.Not.Null);
            Assert.That(generator, Is.InstanceOf<SidePassageGenerator>());
        }

        [Test]
        public void BuildStairsGenerator()
        {
            var generator = areaGeneratorFactory.Build(AreaTypeConstants.Stairs);
            Assert.That(generator, Is.Not.Null);
            Assert.That(generator, Is.InstanceOf<StairsGenerator>());
        }

        [Test]
        public void BuildTurnGenerator()
        {
            var generator = areaGeneratorFactory.Build(AreaTypeConstants.Turn);
            Assert.That(generator, Is.Not.Null);
            Assert.That(generator, Is.InstanceOf<TurnGenerator>());
        }

        [Test]
        public void BuildHallGenerator()
        {
            var generator = areaGeneratorFactory.Build(AreaTypeConstants.Hall);
            Assert.That(generator, Is.Not.Null);
            Assert.That(generator, Is.InstanceOf<HallGenerator>());
        }

        [TestCase(AreaTypeConstants.Chamber, true)]
        [TestCase(AreaTypeConstants.DeadEnd, false)]
        [TestCase(AreaTypeConstants.Door, true)]
        [TestCase(AreaTypeConstants.General, false)]
        [TestCase(AreaTypeConstants.Hall, true)]
        [TestCase(AreaTypeConstants.SidePassage, true)]
        [TestCase(AreaTypeConstants.Stairs, true)]
        [TestCase(AreaTypeConstants.Turn, true)]
        [TestCase("area type", false)]
        public void AreaTypeHasSpecificGenerator(string areaType, bool hasSpecificGenerator)
        {
            Assert.That(areaGeneratorFactory.HasSpecificGenerator(areaType), Is.EqualTo(hasSpecificGenerator));
        }
    }
}

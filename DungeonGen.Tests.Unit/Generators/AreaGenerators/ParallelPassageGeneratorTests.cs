using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.AreaGenerators;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace DungeonGen.Tests.Unit.Generators.AreaGenerators
{
    [TestFixture]
    public class ParallelPassageGeneratorTests
    {
        private AreaGenerator parallelPassageGenerator;
        private Mock<AreaGenerator> mockHallGenerator;

        [SetUp]
        public void Setup()
        {
            mockHallGenerator = new Mock<AreaGenerator>();
            parallelPassageGenerator = new ParallelPassageGenerator(mockHallGenerator.Object);
        }

        [Test]
        public void GeneratorReturnsLeftAndRightHalls()
        {
            var leftHall = new Area();
            var rightHall = new Area();
            mockHallGenerator.SetupSequence(g => g.Generate(9266, 90210)).Returns(new[] { leftHall }).Returns(new[] { rightHall });

            var passages = parallelPassageGenerator.Generate(9266, 90210);
            Assert.That(passages.Count(), Is.EqualTo(2));

            var first = passages.First();
            var last = passages.Last();

            Assert.That(first, Is.EqualTo(leftHall));
            Assert.That(first.Descriptions.Single(), Is.EqualTo(SidePassageConstants.Left90Degrees));
            Assert.That(last, Is.EqualTo(rightHall));
            Assert.That(last.Descriptions.Single(), Is.EqualTo(SidePassageConstants.Right90Degrees));
        }

        [Test]
        public void UseLeftHallWidthIfGreater()
        {
            var leftHall = new Area();
            leftHall.Width = 600;

            var rightHall = new Area();
            rightHall.Width = 42;

            mockHallGenerator.SetupSequence(g => g.Generate(9266, 90210)).Returns(new[] { leftHall }).Returns(new[] { rightHall });

            var passages = parallelPassageGenerator.Generate(9266, 90210);
            Assert.That(passages.Count(), Is.EqualTo(2));

            var first = passages.First();
            var last = passages.Last();

            Assert.That(first, Is.EqualTo(leftHall));
            Assert.That(first.Width, Is.EqualTo(600));
            Assert.That(last, Is.EqualTo(rightHall));
            Assert.That(last.Width, Is.EqualTo(600));
        }

        [Test]
        public void UseRightHallWidthIfGreater()
        {
            var leftHall = new Area();
            leftHall.Width = 42;

            var rightHall = new Area();
            rightHall.Width = 600;

            mockHallGenerator.SetupSequence(g => g.Generate(9266, 90210)).Returns(new[] { leftHall }).Returns(new[] { rightHall });

            var passages = parallelPassageGenerator.Generate(9266, 90210);
            Assert.That(passages.Count(), Is.EqualTo(2));

            var first = passages.First();
            var last = passages.Last();

            Assert.That(first, Is.EqualTo(leftHall));
            Assert.That(first.Width, Is.EqualTo(600));
            Assert.That(last, Is.EqualTo(rightHall));
            Assert.That(last.Width, Is.EqualTo(600));
        }
    }
}

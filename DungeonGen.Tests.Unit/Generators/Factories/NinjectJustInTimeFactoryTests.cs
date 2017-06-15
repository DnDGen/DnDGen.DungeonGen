using DungeonGen.Domain.Generators.ExitGenerators;
using DungeonGen.Domain.Generators.Factories;
using Ninject.MockingKernel;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;

namespace DungeonGen.Tests.Unit.Generators.Factories
{
    [TestFixture]
    public class NinjectJustInTimeFactoryTests
    {
        private JustInTimeFactory justInTimeFactory;
        private MoqMockingKernel mockKernel;

        [SetUp]
        public void Setup()
        {
            mockKernel = new MoqMockingKernel();
            justInTimeFactory = new NinjectJustInTimeFactory(mockKernel);
        }

        [Test]
        public void BuildGenerator()
        {
            mockKernel.Bind<ExitGenerator>().ToMock().InSingletonScope().Named("area type");
            var mockGenerator = mockKernel.GetMock<ExitGenerator>();

            var generator = justInTimeFactory.Build<ExitGenerator>("area type");
            Assert.That(generator, Is.Not.Null);
            Assert.That(generator, Is.EqualTo(mockGenerator.Object));
        }

        [Test]
        public void BuildNamedGenerator()
        {
            mockKernel.Bind<ExitGenerator>().ToMock().InSingletonScope();
            var mockGenerator = mockKernel.GetMock<ExitGenerator>();

            var generator = justInTimeFactory.Build<ExitGenerator>();
            Assert.That(generator, Is.Not.Null);
            Assert.That(generator, Is.EqualTo(mockGenerator.Object));
        }
    }
}

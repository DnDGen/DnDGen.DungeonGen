using DungeonGen.Generators;
using DungeonGen.Generators.Domain.RuntimeFactories;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Bootstrap.Modules
{
    [TestFixture]
    public class GeneratorModuleTests : BootstrapTests
    {
        [Test]
        public void DungeonGeneratorIsInjected()
        {
            AssertInjected<IDungeonGenerator>();
        }

        [Test]
        public void AreaGeneratorFactoryIsInjected()
        {
            AssertInjected<IAreaGeneratorFactory>();
        }

        [Test]
        public void TrapGeneratorIsInjected()
        {
            AssertInjected<ITrapGenerator>();
        }
    }
}

using DungeonGen.Generators;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Bootstrap.Modules
{
    [TestFixture]
    public class GeneratorModuleTests : BootstrapTests
    {
        [Test]
        public void DungeonGeneratorIsNotASingleton()
        {
            AssertNotSingleton<IDungeonGenerator>();
        }
    }
}

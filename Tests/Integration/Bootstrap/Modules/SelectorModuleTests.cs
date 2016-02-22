using DungeonGen.Selectors;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Bootstrap.Modules
{
    [TestFixture]
    public class SelectorModuleTests : BootstrapTests
    {
        [Test]
        public void PercentileSelectorIsNotASingleton()
        {
            AssertNotSingleton<IPercentileSelector>();
        }
    }
}

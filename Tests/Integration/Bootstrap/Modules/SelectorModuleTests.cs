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
            AssertInjected<IPercentileSelector>();
        }

        [Test]
        public void AreaPercentileSelectorIsNotASingleton()
        {
            AssertInjected<IAreaPercentileSelector>();
        }
    }
}

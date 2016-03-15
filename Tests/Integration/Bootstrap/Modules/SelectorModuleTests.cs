using DungeonGen.Selectors;
using DungeonGen.Selectors.Domain;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Bootstrap.Modules
{
    [TestFixture]
    public class SelectorModuleTests : BootstrapTests
    {
        [Test]
        public void PercentileSelectorIsNotASingleton()
        {
            AssertInstanceOf<IPercentileSelector, PercentileSelector>();
        }

        [Test]
        public void AreaPercentileSelectorIsNotASingleton()
        {
            AssertInstanceOf<IAreaPercentileSelector, AreaPercentileSelector>();
        }
    }
}

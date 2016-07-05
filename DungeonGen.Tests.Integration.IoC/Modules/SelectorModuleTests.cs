using DungeonGen.Domain.Selectors;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.IoC.Modules
{
    [TestFixture]
    public class SelectorModuleTests : IoCTests
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

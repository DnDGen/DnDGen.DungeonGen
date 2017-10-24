using DungeonGen.Domain.Selectors;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.IoC.Modules
{
    [TestFixture]
    public class SelectorModuleTests : IoCTests
    {
        [Test]
        public void AreaPercentileSelectorIsNotASingleton()
        {
            AssertInstanceOf<IAreaPercentileSelector, AreaPercentileSelector>();
        }
    }
}

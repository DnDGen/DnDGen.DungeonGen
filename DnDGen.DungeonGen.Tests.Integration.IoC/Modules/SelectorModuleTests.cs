using DnDGen.DungeonGen.Selectors;
using NUnit.Framework;

namespace DnDGen.DungeonGen.Tests.Integration.IoC.Modules
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

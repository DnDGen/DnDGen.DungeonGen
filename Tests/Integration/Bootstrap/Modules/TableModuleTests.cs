using DungeonGen.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Bootstrap.Modules
{
    [TestFixture]
    public class TableModuleTests : BootstrapTests
    {
        [Test]
        public void StreamLoaderIsNotASingleton()
        {
            AssertInjected<StreamLoader>();
        }
    }
}

using DungeonGen.Tables;
using DungeonGen.Tables.Domain;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Bootstrap.Modules
{
    [TestFixture]
    public class TableModuleTests : BootstrapTests
    {
        [Test]
        public void StreamLoaderIsNotASingleton()
        {
            AssertInstanceOf<StreamLoader, EmbeddedResourceStreamLoader>();
        }
    }
}

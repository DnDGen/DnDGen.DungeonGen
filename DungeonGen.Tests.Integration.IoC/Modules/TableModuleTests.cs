using DungeonGen.Domain.Tables;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.IoC.Modules
{
    [TestFixture]
    public class TableModuleTests : IoCTests
    {
        [Test]
        public void StreamLoaderIsNotASingleton()
        {
            AssertInstanceOf<StreamLoader, EmbeddedResourceStreamLoader>();
        }
    }
}

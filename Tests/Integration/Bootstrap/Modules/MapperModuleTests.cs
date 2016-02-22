using DungeonGen.Mappers;
using DungeonGen.Mappers.Domain;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Bootstrap.Modules
{
    [TestFixture]
    public class MapperModuleTests : BootstrapTests
    {
        [Test]
        public void PercentileMapperIsASingleton()
        {
            AssertSingleton<PercentileMapper>();
        }

        [Test]
        public void PercentileMapperHasCachingProxy()
        {
            AssertInstanceOf<PercentileMapper, PercentileMapperCachingProxy>();
        }
    }
}

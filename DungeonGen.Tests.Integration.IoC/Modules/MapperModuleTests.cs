using DungeonGen.Domain.Mappers;
using NUnit.Framework;

namespace DungeonGen.Tests.Integration.IoC.Modules
{
    [TestFixture]
    public class MapperModuleTests : IoCTests
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

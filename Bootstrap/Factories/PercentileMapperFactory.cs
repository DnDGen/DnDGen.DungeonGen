using DungeonGen.Mappers;
using DungeonGen.Mappers.Domain;
using DungeonGen.Tables;
using Ninject;

namespace DungeonGen.Bootstrap.Factories
{
    public static class PercentileMapperFactory
    {
        public static PercentileMapper Create(IKernel kernel)
        {
            var streamLoader = kernel.Get<StreamLoader>();
            PercentileMapper mapper = new PercentileXmlMapper(streamLoader);

            mapper = new PercentileMapperCachingProxy(mapper);

            return mapper;
        }
    }
}

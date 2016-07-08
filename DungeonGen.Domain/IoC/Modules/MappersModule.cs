using DungeonGen.Domain.Mappers;
using Ninject.Modules;

namespace DungeonGen.Domain.IoC.Modules
{
    internal class MappersModule : NinjectModule
    {
        public override void Load()
        {
            Bind<PercentileMapper>().To<PercentileXmlMapper>().WhenInjectedInto<PercentileMapperCachingProxy>();
            Bind<PercentileMapper>().To<PercentileMapperCachingProxy>().InSingletonScope();
        }
    }
}

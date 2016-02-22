using DungeonGen.Bootstrap.Factories;
using DungeonGen.Mappers;
using Ninject.Modules;

namespace DungeonGen.Bootstrap.Modules
{
    public class MappersModule : NinjectModule
    {
        public override void Load()
        {
            Bind<PercentileMapper>().ToMethod(c => PercentileMapperFactory.Create(c.Kernel)).InSingletonScope();
        }
    }
}

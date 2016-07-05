using DungeonGen.Domain.IoC.Factories;
using DungeonGen.Domain.Mappers;
using Ninject.Modules;

namespace DungeonGen.Domain.IoC.Modules
{
    internal class MappersModule : NinjectModule
    {
        public override void Load()
        {
            Bind<PercentileMapper>().ToMethod(c => PercentileMapperFactory.Create(c.Kernel)).InSingletonScope();
        }
    }
}

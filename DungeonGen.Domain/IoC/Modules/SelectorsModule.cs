using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Selectors;
using Ninject.Modules;

namespace DungeonGen.Domain.IoC.Modules
{
    internal class SelectorsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPercentileSelector>().To<PercentileSelector>();
            Bind<IAreaPercentileSelector>().To<AreaPercentileSelector>();
        }
    }
}

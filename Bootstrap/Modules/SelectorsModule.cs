using DungeonGen.Selectors;
using DungeonGen.Selectors.Domain;
using Ninject.Modules;

namespace DungeonGen.Bootstrap.Modules
{
    public class SelectorsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPercentileSelector>().To<PercentileSelector>();
            Bind<IAreaPercentileSelector>().To<AreaPercentileSelector>();
        }
    }
}

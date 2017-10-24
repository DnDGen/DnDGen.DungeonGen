using DungeonGen.Domain.Selectors;
using Ninject.Modules;

namespace DungeonGen.Domain.IoC.Modules
{
    internal class SelectorsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAreaPercentileSelector>().To<AreaPercentileSelector>();
        }
    }
}
